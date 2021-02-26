using System.IO;
using System.Web;
using SvgMediaType.Package.Configuration;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace SvgMediaType.Package.Components
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SvgMediaSavedEventComposer : ComponentComposer<SvgMediaSavedEventComponent>
    {
    }

    public class SvgMediaSavedEventComponent : IComponent
    {
        private const string SvgFileExtension = "svg";

        private readonly IMediaTypeService _mediaTypeService;
        private readonly IMediaService _mediaService;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;

        public SvgMediaSavedEventComponent(IMediaTypeService mediaTypeService, IMediaService mediaService, IContentTypeBaseServiceProvider contentTypeBaseServiceProvider)
        {
            _mediaTypeService = mediaTypeService;
            _mediaService = mediaService;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
        }

        public void Initialize()
        {
            MediaService.Saved += MediaService_Saved;
        }

        public void Terminate()
        {
            MediaService.Saved -= MediaService_Saved;
        }

        private void MediaService_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            foreach (var mediaItem in e.SavedEntities)
            {
                var svgMedia = mediaItem.ContentType.Alias == SvgMediaTypeConfig.SvgMediaTypeAlias ? mediaItem : null;

                if (ShouldChangeContentTypeToSvg(mediaItem))
                    svgMedia = ChangeContentTypeToSvg(mediaItem);

                if (svgMedia == null)
                    continue;

                SetSvgContent(svgMedia);

                _mediaService.Save(svgMedia, 0, false);
            }
        }

        private static bool ShouldChangeContentTypeToSvg(IMedia mediaItem)
        {
            var mediaType = HttpContext.Current.Request.Form["contentTypeAlias"];

            return mediaType == Constants.Conventions.MediaTypes.AutoSelect
                   && mediaItem.ContentType.Alias != SvgMediaTypeConfig.SvgMediaTypeAlias
                   && mediaItem.GetValue<string>(Constants.Conventions.Media.File).EndsWith(SvgFileExtension);

        }

        private IMedia ChangeContentTypeToSvg(IMedia originalMedia)
        {
            var svgMediaType = _mediaTypeService.Get(SvgMediaTypeConfig.SvgMediaTypeAlias);

            if (svgMediaType == null)
                return null;

            var umbracoFilePath = originalMedia.GetValue<string>(Constants.Conventions.Media.File);
            var filePath = IOHelper.MapPath(umbracoFilePath);
            var fileName = Path.GetFileName(filePath);

            var fileInfo = new FileInfo(filePath);
            var fileStream = fileInfo.OpenReadWithRetry();

            var svgMedia = _mediaService.CreateMedia(originalMedia.Name, originalMedia.ParentId, svgMediaType.Alias, originalMedia.WriterId);

            svgMedia.SetValue(_contentTypeBaseServiceProvider, Constants.Conventions.Media.File, fileName, fileStream);
            svgMedia.SetValue(Constants.Conventions.Media.Extension, originalMedia.GetValue<string>(Constants.Conventions.Media.Extension));
            svgMedia.SetValue(Constants.Conventions.Media.Bytes, originalMedia.GetValue<int>(Constants.Conventions.Media.Bytes));

            _mediaService.Delete(originalMedia, 0);

            return svgMedia;
        }

        private static void SetSvgContent(IMedia svgMedia)
        {
            var umbracoFilePath = svgMedia.GetValue<string>(Constants.Conventions.Media.File);
            var filePath = IOHelper.MapPath(umbracoFilePath);

            var byteArray = System.IO.File.ReadAllBytes(filePath);
            var content = System.Text.Encoding.Default.GetString(byteArray);

            svgMedia.SetValue(SvgMediaTypeConfig.SvgMediaTypeContentPropertyAlias, content);
        }
    }
}
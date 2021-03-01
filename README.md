# SVG Media Type

Adds a SVG media type to Umbraco and renders the medias as SVG tags.

When uploading files with a .svg extension in the media archive, the packages creates the file as a SVG Media Type and reads the content of the file and stores it in a property. This makes it possible to render the SVG file as a real SVG element, with all the benefits of SVG files (scalable, color the SVG with CSS etc.).

Umbraco package: https://our.umbraco.org/projects/website-utilities/xxxxxxx

NuGet package: https://xxxxxxxxxx

## Installation

The package can be installed as an Umbraco package or via NuGet.

TODO
 
## Rendering SVG files
 
The package contains an extension method `RenderSvg(this HtmlHelper helper, IPublishedContent publishedContentSvg, string title = null)` on the HtmlHelper. The method takes an IPublishedContent (and an optional title) and returns an HtmlString with the SVG tag.  

Here's an example of how to render a SVG file from a Home page with a media property  of type Media Picker:
 
    @inherits Umbraco.Web.Mvc.UmbracoViewPage<Umbraco.Web.PublishedModels.Home>
    @using SvgMediaType.Helpers

    @Html.RenderSvg(Model.Media, "My SVG title")

It's also possible to check if the media is in fact an SVG Media Type:

    @inherits Umbraco.Web.Mvc.UmbracoViewPage<Umbraco.Web.PublishedModels.Home>
    @using SvgMediaType.Helpers

    @if (Model.Media is Svg)
    {
        @Html.RenderSvg(Model.Media, "My SVG title")
    }
		
## Configuration

No configuration is required. How ever if you want to change the default values you can add the folowing optional keys in the appSettings in your web.config.

- `<add key="SvgMediaType.SvgMediaTypeAlias" value="svg"/>` - the alias of the SVG media type. The default value is 'svg'.
- `<add key="SvgMediaType.SvgMediaTypeContentPropertyAlias" value="content"/>` - the alias of the property on the SVG media type that holds the content of the SVG file. The default value is 'content'.

## Version history

- 0.1.0
    - Initial release
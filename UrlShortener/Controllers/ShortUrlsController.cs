using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShortUrlsController : ControllerBase
{

}

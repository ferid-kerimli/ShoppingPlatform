using Microsoft.AspNetCore.Hosting;

namespace ShoppingPlatform.BLL.Utility;

public static class DirectoryCreator
{
    public static void EnsureProductImagesFolderExists(IWebHostEnvironment env)
    {
        var productImagesPath = Path.Combine(env.WebRootPath, "ProductImages");

        if (!Directory.Exists(productImagesPath))
        {
            Directory.CreateDirectory(productImagesPath);
        }
    }
}
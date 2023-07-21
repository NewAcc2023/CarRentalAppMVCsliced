namespace CarRentalAppMVC
{
	public class ImageManager
	{
		//SAVE AND DELETE IMAGES FROM FOLDER
		public async Task SaveCarImage(IFormFile image, string imageName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}
		}

		public async Task DeleteCarImage(string imageName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete))
			{
				File.Delete(filePath);
			}
		}
	}
}

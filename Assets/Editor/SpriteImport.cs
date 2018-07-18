/*using UnityEngine;
using UnityEditor;

public class CustomImportSettings : AssetPostprocessor {

	int pixelsPerUnit = 32;
	bool mipMapEnabled = false;
	FilterMode filterMode = FilterMode.Point;

	void OnPostprocessTexture(Texture2D texture) {
		TextureImporter ti = (assetImporter as TextureImporter);
		//ti.spritePixelsPerUnit = pixelsPerUnit;
		ti.filterMode = filterMode;

		ti.mipmapEnabled = mipMapEnabled;
		ti.alphaIsTransparency = true;

		TextureImporterSettings importerSettings = new TextureImporterSettings();
		ti.ReadTextureSettings(importerSettings);

		float size = Mathf.Max(texture.width, texture.height);

		int power = 1;
		while (power < size)
			power *= 2;

		power = Mathf.Clamp(power, 32, 8192);

		ti.SetTextureSettings(importerSettings);
	}
}*/
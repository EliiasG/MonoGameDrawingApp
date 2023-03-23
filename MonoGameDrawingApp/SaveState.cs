using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.VectorSprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MonoGameDrawingApp
{
    public static class SaveState
    {
        //cannot be constant, as it does not know the CommonApplicationData on compile time
        public static readonly string SaveDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VectorDrawingApp");
        public static readonly string PalettesDirectory = Path.Join(SaveDirectory, "Palettes");
        public static readonly string ProjectsPath = Path.Join(SaveDirectory, "projects.txt");
        public static readonly string SelectedPalettePath = Path.Join(SaveDirectory, "selectPalette.txt");

        private static ColorPalette _defaultPalette;
        private static GraphicsDevice _graphicsDevice;
        private static string[] _projects;
        private static List<ColorPalette> _colorPalettes;
        private static int _selectedColorPaletteIndex;

        public static IEnumerable<ColorPalette> ColorPalettes => _colorPalettes;

        public static IEnumerable<string> Projects => _projects;

        public static ColorPalette SelectedColorPalette
        {
            get
            {
                if (_selectedColorPaletteIndex >= _colorPalettes.Count)
                {
                    SelectedColorPalette = _defaultPalette;
                }
                return _colorPalettes[_selectedColorPaletteIndex];
            }
            set
            {
                _selectedColorPaletteIndex = _colorPalettes.IndexOf(value);
                File.WriteAllText(SelectedPalettePath, _selectedColorPaletteIndex.ToString());
            }
        }

        public static void Init(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _colorPalettes = new List<ColorPalette>();
            _defaultPalette = _fromTexture(content.Load<Texture2D>("palette"), "default");

            ReloadColorPalettes();
            ReloadProjects();

            if (File.Exists(SelectedPalettePath))
            {
                try
                {
                    _selectedColorPaletteIndex = int.Parse(File.ReadAllText(SelectedPalettePath));
                }
                catch
                {
                    _selectedColorPaletteIndex = 0;
                }
            }
            else
            {
                _selectedColorPaletteIndex = 0;
                File.Create(SelectedPalettePath);
            }
        }

        public static void ReloadProjects()
        {
            if (!File.Exists(ProjectsPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ProjectsPath));
                File.Create(ProjectsPath).Close();
            }
            _projects = File.ReadLines(ProjectsPath).ToArray();
        }

        public static void Remove(string path)
        {
            List<string> lines = File.ReadLines(ProjectsPath).ToList();
            lines.RemoveAll((s) => s == path);
            File.WriteAllLines(ProjectsPath, lines.ToArray());
            ReloadProjects();
        }

        public static void SetFirst(string path)
        {
            List<string> lines = File.ReadLines(ProjectsPath).ToList();
            while (lines.Remove(path)) ;
            lines.Insert(0, path);
            File.WriteAllLines(ProjectsPath, lines.ToArray());
            ReloadProjects();
        }

        public static void ReloadColorPalettes()
        {
            if (!Directory.Exists(PalettesDirectory))
            {
                Directory.CreateDirectory(PalettesDirectory);
            }

            string[] images = Directory.GetFiles(PalettesDirectory, "*.png", SearchOption.AllDirectories);

            if (_colorPalettes.Count > images.Length + 1)
            {
                _colorPalettes.RemoveRange(images.Length + 1, _colorPalettes.Count - images.Length);
            }

            if (_colorPalettes.Count != 0)
            {
                _colorPalettes[0] = _defaultPalette;
            }
            else
            {
                _colorPalettes.Add(_defaultPalette);
            }

            for (int i = 0; i < images.Length; i++)
            {
                string palettePath = images[i];
                string name = Path.GetRelativePath(PalettesDirectory, palettePath);

                string oldName = _colorPalettes.Count <= i ? "" : _colorPalettes[i].Name;
                if (name != oldName)
                {
                    Texture2D image = Texture2D.FromFile(_graphicsDevice, palettePath);
                    _colorPalettes.Insert(i + 1, _fromTexture(image, name));
                }
            }
        }

        private static ColorPalette _fromTexture(Texture2D texture, string name)
        {
            List<Color> colorList = new();
            ISet<Color> colors = new HashSet<Color>();

            Microsoft.Xna.Framework.Color[] colorData = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];

            texture.GetData(colorData);

            foreach (Microsoft.Xna.Framework.Color color in colorData)
            {
                Color drawingColor = Util.ToDrawingColor(color);
                if (colors.Contains(drawingColor))
                {
                    continue;
                }
                colors.Add(drawingColor);
                colorList.Add(drawingColor);
            }

            return new ColorPalette(colorList, name);
        }
    }
}

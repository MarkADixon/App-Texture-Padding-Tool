using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace PadTexture
{
    public partial class TextureInterface : Form
    {
        public Game1 game;
        public string defaultFilePath = Application.StartupPath;
        public string fileName;
        

        

        public TextureInterface()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = defaultFilePath;
            openFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    string textureFilePath = openFileDialog1.FileName;
                    myStream.Close();

                   string[] stringSeparators = new string[] { @"\" };
                   string[] result = textureFilePath.Split(stringSeparators,
                   StringSplitOptions.RemoveEmptyEntries);
                   fileName = result[result.Length - 1];

                   // string[] stringSeparators2 = new string[] { fileNameMinusTheFileType };
                   // string[] result2 = textureFilePath.Split(stringSeparators2,
                   // StringSplitOptions.RemoveEmptyEntries);
                   // string pathMinusTheFileName = result2[0];

                    FileStream fileStream = new FileStream(textureFilePath, FileMode.Open);
                    game.inputTexture = Texture2D.FromStream(game.GraphicsDevice, fileStream);
                    fileStream.Close();
                    game.displayTexture = game.inputTexture;

                    label2.Text = textureFilePath;
                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (game.inputTexture == null) return;
            Microsoft.Xna.Framework.Color[] newTextureData;
            Microsoft.Xna.Framework.Color[] inputTextureData;
            inputTextureData = new Microsoft.Xna.Framework.Color[game.inputTexture.Width * game.inputTexture.Height];
            newTextureData = new Microsoft.Xna.Framework.Color[(game.inputTexture.Width + 4) * (game.inputTexture.Height + 4)];
            game.inputTexture.GetData(inputTextureData);

            //copy the input texture data to the new data (with space for the padding)
            for (int x = 0; x < game.inputTexture.Width; x++)
            {
                for (int y = 0; y < game.inputTexture.Height; y++)
                {
                    newTextureData[(x+2) + ((y+2) * (game.inputTexture.Width+4))] = inputTextureData[x + (y * game.inputTexture.Width)];
                }
            }

            for (int x = 0; x < game.inputTexture.Width + 4; x++)
            {
                if (x == 0 || x ==1)
                {
                    newTextureData[x] = inputTextureData[0];
                    newTextureData[x + game.inputTexture.Width + 4] = inputTextureData[0];
                    newTextureData[x + ((game.inputTexture.Width + 4) * (game.inputTexture.Height + 2))] = inputTextureData[(game.inputTexture.Width * (game.inputTexture.Height - 1))];
                    newTextureData[x + ((game.inputTexture.Width + 4) * (game.inputTexture.Height + 3))] = inputTextureData[(game.inputTexture.Width * (game.inputTexture.Height - 1))];
                    
                }
                if ( x > 1 && x < game.inputTexture.Width +2)
                {
                    newTextureData[x] = inputTextureData[x - 2];
                    newTextureData[x + game.inputTexture.Width + 4] = inputTextureData[x-2];
                    newTextureData[x + ((game.inputTexture.Width + 4) * (game.inputTexture.Height + 2))] = inputTextureData[(x - 2) + (game.inputTexture.Width * (game.inputTexture.Height-1))];
                    newTextureData[x + ((game.inputTexture.Width + 4) * (game.inputTexture.Height + 3))] = inputTextureData[(x - 2) + (game.inputTexture.Width * (game.inputTexture.Height - 1))];
                }
                if (x == game.inputTexture.Width + 2 || x == game.inputTexture.Width + 3)
                {
                   newTextureData[x] = inputTextureData[game.inputTexture.Width-1];
                   newTextureData[x + game.inputTexture.Width + 4] = inputTextureData[game.inputTexture.Width - 1];
                   newTextureData[x + ((game.inputTexture.Width + 4) * (game.inputTexture.Height + 2))] = inputTextureData[(game.inputTexture.Width - 1) + (game.inputTexture.Width * (game.inputTexture.Height - 1))];
                   newTextureData[x + ((game.inputTexture.Width + 4) * (game.inputTexture.Height + 3))] = inputTextureData[(game.inputTexture.Width - 1) + (game.inputTexture.Width * (game.inputTexture.Height - 1))];
                }
                
            }

            for (int y = 2; y < game.inputTexture.Height +2; y++)
            {
                newTextureData[0 + (y * (game.inputTexture.Width + 4))] = inputTextureData[(y-2) * game.inputTexture.Width];
                newTextureData[1 + (y * (game.inputTexture.Width + 4))] = inputTextureData[(y-2) * game.inputTexture.Width];
                newTextureData[game.inputTexture.Width + 2 + (y * (game.inputTexture.Width + 4))] = inputTextureData[ (game.inputTexture.Width - 1) + ((y-2) * game.inputTexture.Width)];
                newTextureData[game.inputTexture.Width + 3 + (y * (game.inputTexture.Width + 4))] = inputTextureData[ (game.inputTexture.Width - 1) + ((y-2) * game.inputTexture.Width)];
            }

            Texture2D outputTexture = new Texture2D(game.GraphicsDevice,game.inputTexture.Width+4,game.inputTexture.Height+4);
            outputTexture.SetData(newTextureData);
            game.displayTexture = outputTexture;

            FileStream fileStream = new FileStream(Application.StartupPath+@"\"+ fileName, FileMode.Create, FileAccess.ReadWrite);
            outputTexture.SaveAsPng(fileStream, outputTexture.Width, outputTexture.Height);
            fileStream.Close();
            

        }

        private void TextureInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}

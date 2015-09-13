using System;


namespace PadTexture
{
#if WINDOWS
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            TextureInterface form = new TextureInterface();
            form.Show();
            form.game = new Game1(
                form.pictureBox.Handle,
                form,
                form.pictureBox);

            form.game.Run();
        }
    }
#endif
}
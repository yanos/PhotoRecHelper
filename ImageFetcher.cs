using System;
using System.Drawing;
using System.IO;

namespace PhotoRecHelper
{
    public static class ImageFetcher
    {
        const string _destSuffix = "img";

        public static void Fetch( string src, string dest, string[] supportedExts, int minWidth, int minHeight, bool moveFile )
        {
            var op = moveFile ? "Moving" : "Copying";

            var targetDir = Path.Combine( dest, _destSuffix );
            if (!Directory.Exists( targetDir ))
            {
                Directory.CreateDirectory( targetDir );
            }

            foreach (var dir in Directory.GetDirectories( src ))
            {
                foreach (var file in Directory.GetFiles( dir ))
                {
                    var ext = Path.GetExtension( file );
                    if (Array.IndexOf( supportedExts, ext.ToLower() ) != -1 )
                    {
                        try
                        {
                            Console.WriteLine( $"{op} {file}..." );

                            var img = Image.FromFile( file, false );
                            var width = img.Width;
                            var height = img.Height;
                            img.Dispose();

                            if (width >= minWidth && height >= minHeight)
	                        {
                                var fullDestPath = Path.Combine( targetDir, Path.GetFileName( file ) );
                                while (File.Exists( fullDestPath ))
                                    fullDestPath = Path.Combine( Path.GetDirectoryName( fullDestPath ), 
                                                                 Path.GetFileNameWithoutExtension( fullDestPath ) + "_dup" + Path.GetExtension( fullDestPath ) );

	                            if (moveFile)
	                                File.Move( file, fullDestPath );
	                            else
	                                File.Copy( file, fullDestPath );
	                        }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine( $"Exception occured when dealing with {file}: {e.Message}" );
                        }
                    }
                }
            }
        }               
    }
}

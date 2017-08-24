using System;
using System.IO;

namespace PhotoRecHelper
{
    public class FileAggregator
    {
        public static void Aggregate( string src, string dest, string[] extBlacklist, bool moveFile )
        {
            var op = moveFile ? "Moving" : "Copying";

            foreach (var dir in Directory.GetDirectories( src ))
            {
                foreach (var file in Directory.GetFiles( dir ))
                {
                    var ext = Path.GetExtension( file );
                    if (Array.IndexOf( extBlacklist, ext.ToLower() ) != -1)
                        continue;

                    try
                    {
                        Console.WriteLine( $"{op} {file}..." );

                        var targetDir = Path.Combine( dest, ext.Substring( 1 ) );
                        if (!Directory.Exists( targetDir ))
                        {
                            Directory.CreateDirectory( targetDir );
                        }

                        var fullDestPath = Path.Combine( targetDir, Path.GetFileName( file ) );

                        while (File.Exists( fullDestPath ))
                            fullDestPath = Path.Combine( Path.GetDirectoryName( fullDestPath ), 
                                                        Path.GetFileNameWithoutExtension( fullDestPath ) + "_dup" + Path.GetExtension( fullDestPath ) );

                        if (moveFile)
                            File.Move( file, fullDestPath );
                        else
                            File.Copy( file, fullDestPath );
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

using System;
using System.IO;

using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace PhotoRecHelper
{
    public static class ArchiveExtractor
    {
        public static void Extract( string path, string[] supportedExts, bool forceExtract = false )
        {
            foreach (var dir in Directory.GetDirectories( path ))
            {
                foreach (var file in Directory.GetFiles( dir ))
                {
                    var ext = Path.GetExtension( file );
                    if (Array.IndexOf( supportedExts, ext.ToLower() ) != -1 )
                    {
                        try
                        {
	                        Console.WriteLine( $"Extracting {file}..." );

	                        var newDir = Path.Combine( dir, Path.GetFileName( file ) ) + "__PhotoRecHelper";
	                        if (Directory.Exists( newDir ))
	                        {
	                            if (forceExtract)
	                            {
	                                Directory.Delete( newDir );
	                            }
	                            else
	                            {
	                                continue;
	                            }
	                        }

	                        Directory.CreateDirectory( newDir );

	                        switch (ext)
	                        {
	                            case ".zip":
	                                var fastZip = new FastZip();
	                                fastZip.ExtractZip( file, newDir, null );
	                                break;
	                            
								case ".gzip":
	                            case ".gz":
                                    Console.WriteLine( ".gzip and .gz not implemented yet." );
	                                break;

	                            case ".bzip2":
                                    Console.WriteLine( ".bzip2 not implemented yet." );
	                                break;

	                            case ".tar":
                                    var inStream = File.OpenRead( file );
                                    var tarArchive = TarArchive.CreateInputTarArchive(inStream);
                                    tarArchive.ExtractContents( newDir );
                                    tarArchive.Close();
                                    inStream.Close();
	                                break;

	                            default:
                                    Console.WriteLine( "Wtf just happend?" );
	                                continue;

	                        }
	                    }
                        catch (Exception e)
                        {
                            Console.WriteLine( $"Error extracting {file}: {e.Message}" );
                        }
                    }
                }
            }

        }
    }
}

using System;
using System.IO;
using System.Linq;

using Mono.Options;

namespace PhotoRecHelper
{
    class PhotoRecHelper
    {
        const string    _defaultDest           = "./";
        const int       _defaultMinImgHeight   = 300;
        const int       _defaultMinImgWidth    = 300;
        static string[] _supportedArchiveExts  = { ".zip", /*".gzip", ".gz", ".bzip2",*/ ".tar" };
        static string[] _supportedImageExts    = { ".jpg", ".jpeg", ".bmp", ".gif", ".png", ".tiff" };

        static bool     _showHelp;
        static string   _src;
        static string   _dest           = _defaultDest;
        static bool     _forceExtract;
        static bool     _moveFile;
        static int      _minImgWidth    = _defaultMinImgWidth;
        static int      _minImgHeight   = _defaultMinImgHeight;

        public static void Main( string[] args )
        {
            var optSet = MakeOptionSet();
            optSet.Parse( args );

            if (_showHelp || string.IsNullOrEmpty( _src ))
            {
                optSet.WriteOptionDescriptions( Console.Out );
                Environment.Exit( _showHelp ? 0 : -1 );
            }

            if (!Directory.Exists( _src ))
            {
                Console.WriteLine( "Error: source path doean't exist." );
                Environment.Exit( -1 );
            }

            Console.WriteLine( "Pass 1: Super Extracter" );
            ArchiveExtractor.Extract( _src, _supportedArchiveExts, _forceExtract );

            Console.WriteLine( "Pass 2: Super Image Fetcher" );
            ImageFetcher.Fetch( _src, _dest, _supportedImageExts, _minImgWidth, _minImgHeight, _moveFile );
            //SuperMiscFileFetcher.Fetch( _src, _dest, _miscExtensions, _moveFile );

            Console.WriteLine( "Pass 3: Super File Aggregator" );
            FileAggregator.Aggregate( _src, _dest, _supportedArchiveExts.Concat( _supportedImageExts ).ToArray(), _moveFile );

            Console.WriteLine( "Done!" );
        }

        static OptionSet MakeOptionSet()
        {
            return new OptionSet()
            {
                { 
                    "x|force-extract", 
                    "Force extract every archive (default is to ignore already extracted archive from a previous run.)", 
                    v => _forceExtract = true 
                },

                { 
                    "m|move-file", 
                    "Move files to destination instead of copying",
                    v => _moveFile = true
                },

                { 
                    "mh|min-img-height=", 
                    $"Only consider image that have at least this height (default {_defaultMinImgHeight})",
                    (int v) => _minImgHeight = v 
                },

                { 
                    "mw|min-img-width=", 
                    $"Only consider image that have at least this width (default {_defaultMinImgWidth})",
                    (int v) => _minImgWidth = v 
                },

                /*{ 
                    "ext|misc-extensions=", 
                    $"A comma separated list of extensions to fetch (default {string.Join( ", ", _defaultMiscExtensions)})",
                    v => _miscExtensions = v.Split( ',' )
                },*/

                { 
                    "h|help", 
                    "show this message and exit", 
                    v => _showHelp = true 
                },

                { 
                    "<>", 
                    v => 
                    {
                        if (string.IsNullOrEmpty( _src ))
                            _src = v;
                        else if (_dest == _defaultDest)
                        {
                            _dest = v;
                            if (!Directory.Exists( _dest ))
                            {
                                Directory.CreateDirectory( _dest );
                            }
                        }
                        else
                        {
                            Console.WriteLine( $"Error: Source and destination already set, don't know what to do with {v}." );
                            Environment.Exit( -1 );
                        }
                    }
                }
            };
        }
    }
}

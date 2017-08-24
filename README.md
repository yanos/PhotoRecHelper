# PhotoRecHelper
Helper to fetch various files types from a [PhotoRec](http://www.cgsecurity.org/wiki/PhotoRec) directory structure.

This tool is meant to be run against a [PhotoRec](http://www.cgsecurity.org/wiki/PhotoRec) directory.

# This helper will:

1. Extract all archive that it can find (currently supporting zip and tar).
2. Move (or copy) all supported image files (jpg, bmp, gif, png, and tiff) with the specified minimum resolution into a img directory.
3. Move (or Copy) all remaining files into a directory based on it's extension.

# Usage

mono PhtotoRecHelper.exe \<source directory\> \<output directory\> \<options\>

# Options
```
-x, --force-extract          Force extract every archive (default is to ignore
                             already extracted archive from a previous run.)

-m, --move-file              Move files to destination instead of copying

-mh, --min-img-height=VALUE  Only consider image that have at least this height (default 300)

-mw, --min-img-width=VALUE   Only consider image that have at least this width (default 300)

-h, --help                   show this message and exit
```

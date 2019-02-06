set source=C:\Users\luole\LittleDeity\Assets\Art\Tiles\buried_2_orig.png
set dest=C:\Users\luole\LittleDeity\Assets\Art\Tiles\buried_2.png
set newWidth=340
set newHeight=340

call .\scale.bat -source %source% -target %dest%  -max-height %newHeight% -max-width %newWidth% -keep-ratio yes -force yes
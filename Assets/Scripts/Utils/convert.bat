@echo off
setlocal enabledelayedexpansion

REM Loop through all .mp4 files in the folder
for %%f in (*.mp4) do (
    REM Get the file name without the extension
    set "filename=%%~nf"
    
    REM Create the new filename with _tmp appended
    set "tmpfilename=!filename!_tmp.mp4"
    
    REM Rename the original file to add _tmp
    ren "%%f" "!tmpfilename!"
    
    REM Output the conversion process
    echo Processing "!tmpfilename!" -> "%%f"
    
    REM Run the ffmpeg command with the -y flag to auto confirm overwrite
    ffmpeg -y -i "!tmpfilename!" -c:v libx265 -pix_fmt yuv420p "%%f"
)

echo All files processed.
pause

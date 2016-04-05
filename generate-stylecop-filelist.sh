#!/bin/sh
# Script: generate-stylecop-filelist.sh
# Description:
# Returns all .cs files within a specific directory, wrapped in StyleCop XML
# to be inserted into Settings.StyleCop.

STYLECOP_SETTINGS="Settings.StyleCop"
OUTFILE="StyleCopFileList.xml"
SEARCH="*.cs"

XMLSTART="  <SourceFileList>\n"
XMLFOOTER="
    <Settings>
      <GlobalSettings>
        <BooleanProperty Name=\"RulesEnabledByDefault\">False</BooleanProperty>
      </GlobalSettings>
    </Settings>
  </SourceFileList>"

function usage() {
    echo "Usage: generate-stylecop-filelist.sh directory"
    echo "Ex: ./generate-stylecop-filelist.sh Assets/RedBlueGames"
    echo "---"
    echo "Once run, copy the text from $OUTFILE to your $STYLECOP_SETTINGS file."
}

# Main body of script starts here
function main() {
  echo "Searching for files named $SEARCH inside $1..."
  find $1 -name $SEARCH -printf "    <SourceFile>%f</SourceFile>\n" | xargs -0 -I {} printf "$XMLSTART{}$XMLFOOTER" > $OUTFILE
  echo "Saved file list xml to $OUTFILE."
  echo "----"
  echo "To apply these ignores to StyleCop, copy the contents of $OUTFILE"
  echo "inside the <StyleCopSettings> tags within $STYLECOP_SETTINGS"
}

if [ "$#" -eq 1 ]; then
    main $1
else
    usage
    exit
fi

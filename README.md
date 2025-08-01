<center><img src="https://raw.githubusercontent.com/KnightNine/ManorDB/master/Images/logowback.png" alt="Logo" width="400" > </center>
<hr>
<p>
Open Source Json Database Manager Made in WinForms that copies <a href = "https://github.com/ncannasse/castle">CastleDB's</a> UI structure. 
This Database Manager also has features which are unique to it for the purpose of further automating database management, features like: adding additional restrictions to how data can be entered, automatically constructed tables defined via "table scripts", and referencing data from non-database files via regex.
</p>

<h2>What it looks like :</h2>
<center><img src="https://raw.githubusercontent.com/KnightNine/ManorDB/master/Images/darkmode.PNG" alt="Logo" width="900" > </center>


<p>
<b>Q:</b> Why did I create this when CastleDB already exists?
</p>
<p>
<b>A:</b> Because CastleDB has/had bugs that creep in when the database reaches a certain level of complexity which made the 3000 line json file I had unreadable at seemingly random, happened 3 times before I decided not to stick around and find out what the exact issue was and just make my own DB manager that actually works. And so I won't have to sift through 3000 lines of json script to fix the issue as there is no indication of what line the read error occurred at.

The game I am working on relies on complex repeating data structures to define its entities therefore I find that this Database manager would be particularly useful to me.
</p>



<hr>
<h2>Goals/Nice-to-Haves:</h2>
<p>
  There are some features that should be added beyond basic functionality but my attention is needed elsewhere. Here is what MDB is lacking in order of most prioritized to least:
<p/>
<ul>
  <li>ability to change columns linked to a `Auto Table Constructor Script Receiver` column without having to delete and re-add the column (thus losing data)</li>
  <li>hovering over the table tabs should allow you to scroll through them with the scroll wheel</li>
  <li>ability to copy a row from one subtable and paste it into another subtable of the same depth</li>
  <li>Column Type Icons (to show the type of each column)</li>
  <li> make the row being hovered over become more visible  </li>
  <li>Search functionality for key reference dropdowns</li>
  <li>Editable "on Mouse Hover" Column Descriptions + text editor for text cells</li>
  <li>"Table Directory" column type that constructs and stores a string array of the directory in another table's structure, this would resolve the typos that could arise from writing these directories manually.</li>
  <li>Key Shortcuts</li>
  <li>Image Columns</li>
  <li>File Directory Primary Key List Table Type (a table that refrences a file directory and generates a table with the file names as row data for a Primary Key column.)</li>
  <li>Undo/Redo</li>
  <li>an option to expand all rows so that text is readable</li>
  <li> Normalized Numeric Column (all values are restricted to total 1 across all rows in the same table/sub-table)</li>
  <li>A "Foreign Key Refrence Primary Key" column (to base a table around foreign keys that can't have duplicate refrences)</li>
  <li>A "force adjecent cell to value when cell is filled" setting, similar to the feature that disallows two cells from being filled at the same time, except instead of disabling the other cell entirely, the cell is set to a specific value and becomes read only. </li>
  <li>Table Shifting (re-ordering of Tables)</li>
  <li>Further Organization of Code (in progress)</li>
  <li>Detailed Commentation (in progress)</li>
  <li>Export to Index Removal (removal of indexes that store the order of rows in the dictionary and replacing them with that row's primary key, in order to further simplify data)</li>
  <li>Separators (equivalent to CDB's "separator")</li>
  <li>Column Input Limiter (An option to limit the number of rows that can be filled within a certain column before the rest become disabled)</li>
 
 
</ul>


<h2>Documentation:</h2>
<a href= "https://github.com/KnightNine/ManorDB/wiki"> Check out the wiki for more info! </a>


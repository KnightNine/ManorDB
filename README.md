

<center><img src="https://raw.githubusercontent.com/KnightNine/ManorDB/master/Images/MDB%20Full%20Text.png" alt="Logo" width="400" > </center>
<hr>
<p>
Open Source Json Database Manager Made in WinForms that copies <a href = "https://github.com/ncannasse/castle">CastleDB's</a> UI structure.
</p>

<p>What it looks like:</p>
<center><img src="https://raw.githubusercontent.com/KnightNine/ManorDB/master/Images/Sample%20Image.PNG" alt="Logo" width="900" > </center>

<p>
<b>Q:</b> Why did I create this when CastleDB already exists?
</p>
<p>
<b>A:</b> Because CastleDB has/had bugs that creep in when the database reaches a certain level of complexity which made the 3000 line json file I had unreadable at seemingly random, happened 3 times before I decided not to stick around and find out what the exact issue was and just make my own DB manager that actually works and so I won't have to sift through 3000 lines of json script to fix the issue as there is no indication of what line the read error occurred at.

The game I am working on relies on complex repeating data structures to define its entities therefore I find that this Database manager would be particularly useful to me.
</p>
<hr>
<h4>FeatureList:</h4>
<ul>
  <li><b>Column Types:</b></li>
  <ul>
    <li>Primary Key</li>
    <li>Text</li>
    <li>Numerical</li>
    <li>Bool</li>
    <li>Subtable (the equivalent of CDB's "list" column type)</li>
    <li>Foreign Key Reference (the equivalent of CDB's "Reference" column type)</li>
    <li><a href = "https://github.com/KnightNine/ManorDB/releases/tag/v1.5.12-beta">Parent Subtable Foreign Key Reference</a></li>
  </ul>
  <li><b>Object Oriented Data:</b></li>
  <ul>
    <li>All data of a row entry is contained within that entry's data as opposed to Table Oriented Data, in which the data of one entity is stored across multiple tables through references, (only exception being foreign key reference entries that are explicitly references), and as such, all data from a single entity can be loaded without calling multiple tables , this is mostly relevant to subtables. I want to keep all overhead data used by the manager that is not relevant to the game reading a single entity's data, out of the way. It is most likely that game data is only needed on a per entity basis that this storage format would work well with. (pretty sure cdb's data is stored in the same way)
    </li>
  </ul>
  <li><b>Why Use this Manager Over CDB?:</b></li>
  <ul>
    <li><p>There's also a few benefits to using this DB Manager over CastleDB, one being that I don't store whether or not subtables are hidden within the data itself as to not have extraneous data within the database that's weaved into the data you actually want to use. I currently don't save whether or not tables are hidden (so all subtables are closed when switching between tables) but if i did it would be stored in an external table that can be cut from the database when you only want to use the data.</p> <p>Second benefit being that you can hide columns to better see the columns you want to edit if you have a wide and shallow data model that stores many columns across one row so there's no need for unnecessary categorisation just so the data can be more easily viewed in the editor (this is only for the main table columns and not for subtables).</p> <p>An issue I had with CDB was that there was a limited number of tabs for switching between tables before they trailed off the window and became inaccessible via the mouse and required the use of key shortcuts, isn't an issue here.</p> <p> If you want to do manual edits to your database you can see where the read error occured at if there was one.</p> <p> Lastly; there's are some features unique to MDB: </p>
<ul>
 You can clarify columns that aren't meant to be filled out at the same time <a href = "https://github.com/KnightNine/ManorDB/releases/tag/v1.4.11-beta">Adjacent Column Disabler Settings</a> </ul> 
<ul>You can have a <a href = "https://github.com/KnightNine/ManorDB/releases/tag/v1.5.12-beta"> foreign key reference column that is linked to a parent subtable</a>. </ul>
    </li>
  </ul>
</ul>

<hr>
<h4>Goals/Nice-to-Haves:</h4>
<p>
  There are some minor features that should be added beyond basic functionality but my attention is needed elsewhere. Here is what MDB is lacking in order of most prioritized to least (the first three may or may not be added soon):
<p/>
<ul>
  <li> Subtable Button Text contains the data of the first few rows within that subtable so that it is viewable on hover </li>
  <li>Editable "on Mouse Hover" Column Descriptions + text editor for text cells</li>
  <li>Image Columns</li>
  <li>Undo/Redo</li>
  <li>Int Restriction Setting on Numeric Column</li>
  <li> an option to set a subtable to be restricted to a number of rows </li>
  <li>A "Foreign Key Refrence Primary Key" column (to base a table around foreign keys that can't have duplicate refrences)</li>
  <li>A "force adjecent cell to value when cell is filled" setting, similar to the feature that disallows two cells from being filled at the same time, except instead of disabling the other cell entirely, the cell is set to a specific value and becomes read only. </li>
  <li>"File Regex Refrence Column": What if you wanted to refrence functions of a script? wouldn't it be much more convenient to have the database manager scan through that script file and update its data automatically. For each column you'd need to define layers of regex on what you want to search for within the script file and the table would create rows and fill in the extracted data for you.</li>
  <li>Key Shortcuts</li>
  <li>Table Shifting (re-ordering of Tables)</li>
  <li>Further Organization of Code</li>
  <li>Detailed Commentation</li>
  <li>Export to Index Removal (removal of indexes that store the order of rows in the dictionary and replacing them with that row's primary key, in order to further simplify data)</li>
  <li>Separators (equivalent to CDB's "separator")</li>
 
 
</ul>
<hr>
<h4>Accessing The Database:</h4>
<p>
All data regarding your row entries is stored within the MDB file's <b>main table</b> name `Database[*tableName*]["@RowEntries"][*index*]` and from there that row's column entries are accessed via `[*columnName*]` . If that data is a Subtable, its row data is accessed in the same fashion through `[*index*]`.
<p/>
<h5>Take Note:</h5>
<p>
Note that for all cell types besides subtables and bools, their value in the database is null if they are left blank. Also the indexes of rows in the database are strings for some reason, it could have something to do with the Newtonsoft package i am using.
</p>
<p>
The main tables are currently stored in alphabetical order and will reorder themselves upon reloading the database. 
</p>
<p>
The data is unindented which i've found might affect other json deserialization methods, in particular, The <a href = "https://godotengine.org/">Godot</a> engine doesn't deserialize unindented json properly as of now, so you can use https://json-indent.com/ to indent the data for that or just to view the database.
<p/>
<p>
while debugging ctrl+c and ctrl+v will throw an exception if done within the node of a subtable.
run without debugging or build the project to resolve this.
<p/>

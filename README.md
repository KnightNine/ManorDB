

<center><img src="https://raw.githubusercontent.com/KnightNine/ManorDB/master/Images/MDB%20Full%20Text.png" alt="Logo" width="400" > </center>
<hr>
<p>
Open Source Json Database Manager Made in WinForms that copies <a href = "https://github.com/ncannasse/castle">CastleDB's</a> UI structure.
</p>


<p>
<b>Q:</b> Why did I create this when CastleDB already exists?
</p>
<p>
<b>A:</b> Because CastleDB has/had bugs that creep in when the database reaches a certain level of complexity and which make the 3000 line json file I had unreadable at random, happened 3 times before I decided not to stick around and find out what the exact issue was and just make my own DB manager that actually works and so I won't have to sift through 3000 lines of json script to fix the issue as there is no indication of what line the read error occurred at. If I want something done right looks like i'll have to do it myself, disappointingly.

The game I am working on relies on complex repeating data structures to define it's entities therefore I find that this Database manager would be particularly useful to me.
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
  </ul>
  <li><b>Object Oriented Data:</b></li>
  <ul>
    <li>All data of a row entry is contained within that entry's data as opposed to Table Oriented Data, in which the data of one entity is stored across multiple tables through references, (only exception being foreign key reference entries that are explicitly references), and as such, all data from a single entity can be loaded without calling multiple tables , this is mostly relevant to subtables. I want to keep all overhead data used by the manager that is not relevant to the game reading a single entity's data, out of the way. It is most likely that game data is only needed on a per entity basis that this storage format would work well with. (pretty sure cdb's data is stored in the same way)
    </li>
  </ul>
  <li><b>Why Use this Manager Over CDB?:</b></li>
  <ul>
    <li><p>There's also a few benefits to using this DB Manager over CastleDB, one being that I don't store whether or not subtables are hidden within the data itself as to not have extraneous data within the database that's weaved into the data you actually want to use. I currently don't save whether or not tables are hidden (so all subtables are closed when switching between tables) but if i did it would be stored in an external table that can be cut from the database when you only want to use the data.</p> <p>Second benefit being that you can hide columns to better see the columns you want to edit if you have a wide and shallow data model that stores many columns across one row so there's no need for unnecessary categorisation just so the data can be more easily viewed in the editor.</p> <p>An issue I had with CDB was that there was a limited number of tabs for switching between tables before they trailed off the window and became inaccessible via the mouse and required the use of key shortcuts, isn't an issue here.</p> <p> Lastly There's a feature that clarifies columns that aren't meant to be filled out at the same time <a href = "https://github.com/KnightNine/ManorDB/releases/tag/v1.4.11-beta">Adjacent Column Disabler Settings</a> </p>
    </li>
  </ul>
</ul>

<hr>
<h4>Goals:</h4>
<p>
  There are a minor features that should be added beyond basic functionality but my attention is needed elsewhere. Here is what MDB is lacking in in order of most prioritized to least (the first three may or may not be added soon):
<p/>
<ul>
  <li>Renaming Tables and Columns</li>
  <li>Table Shifting (re-ordering of Tables)</li>
  <li>Further Organization of Code</li>
  <li>Detailed Commentation</li>
  <li>Export to Index Removal (removal of indexes that store the order of rows in the dictionary and replacing them with that row's primary key, in order to further simplify data)</li>
  <li>Separators (equivalent to CDB's "separator")</li>
 
 
</ul>
<hr>
<h4>Accessing The Database:</h4>
<p>
All data regarding your row entries is stored within the MDB file's main table name `Database[*tableName*]["@RowEntries"][*index*]` and from there that row's column entries are accessed via `[*columnName*]` . If that data is a Subtable, it's row data is accessed in the same fashion through `[*index*]`.
<p/>


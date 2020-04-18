

<center><img src="https://raw.githubusercontent.com/KnightNine/FortressDB/master/Images/FDB_Full_Text.png" alt="Logo" width="400" > </center>
<hr>
<p>
Open Source Database Manager Made in WinForms that copies CastleDB's UI structure.
Still working on a <a href="https://stackoverflow.com/questions/61284153/how-would-i-go-about-creating-a-dropdown-subtable-sub-datagridview-in-winform">way to have subTables</a>
After i'm done that along with saving/loading I'll be posting the application files.
</p>


<p>
<b>Q:</b> Why did I create this when CastleDB already exists?
</p>
<p>
<b>A:</b> Because CastleDB has/had bugs that creep in when the database reaches a certain level of complexity and which make the 3000 line json file I had unreadable at random, happened 3 times before I decided not to stick around and find out what the exact issue was and just make my own DB manager that actually works and so I won't have to sift through 3000 lines of json script to fix the issue as there is no indication of what line the read error occured at. If I want something done right looks like i'll have to do it myself, dissapointingly.
</p>
<hr>
<b>FeatureList/Goals:</b> 
<ul>
  <li><b>Column Types:</b></li>
  <ul>
    <li>Primary Key</li>
    <li>Text</li>
    <li>Numerical</li>
    <li>Bool</li>
    <li>Subtable (the equivalent of CDB's "list" column type)</li>
    <li>Foreign Key Refrence (the equivalent of CDB's "Refrence" column type)</li>
  </ul>
  <li><b>Self Contained Entity Data Format:</b></li>
  <ul>
    <li>All data of a row entry is contained within that entry's data (only exception being foreign key refrence entries that are explicily refrences), and as such, all data from a single entity can be loaded without calling multiple tables, this is mostly relevant to subtables. (i.e. Database>Table>Rows>EntryKey>subtableColumnName>ColumnName>data <b>, as opposed to,</b> (Database>Table>Rows>EntryKey>subtableRefrenceKey /and then/ Database>Subtable>subtableRefrenceKey>data)) I want to keep all overhead data used by the manager that is not relevant to the game reading a single entity's data, out of the way. It is most likely that game data is only needed on a per entity basis that this storage format would work well with.</li>
  </ul>
</ul> 

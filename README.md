

<center><img src="https://raw.githubusercontent.com/KnightNine/FortressDB/master/Images/FDB_Full_Text.png" alt="Logo" width="400" > </center>
<hr>
<p>
Open Source Json Database Manager Made in WinForms that copies <a href = "https://github.com/ncannasse/castle">CastleDB's</a> UI structure.
</p>


<p>
<b>Q:</b> Why did I create this when CastleDB already exists?
</p>
<p>
<b>A:</b> Because CastleDB has/had bugs that creep in when the database reaches a certain level of complexity and which make the 3000 line json file I had unreadable at random, happened 3 times before I decided not to stick around and find out what the exact issue was and just make my own DB manager that actually works and so I won't have to sift through 3000 lines of json script to fix the issue as there is no indication of what line the read error occured at. If I want something done right looks like i'll have to do it myself, dissapointingly.

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
    <li>Foreign Key Refrence (the equivalent of CDB's "Refrence" column type)</li>
  </ul>
  <li><b>Object Oriented Data:</b></li>
  <ul>
    <li>All data of a row entry is contained within that entry's data as opposed to Table Oriented Data, in which the data of one entity is stored across multiple tables through refrences, (only exception being foreign key refrence entries that are explicily refrences), and as such, all data from a single entity can be loaded without calling multiple tables , this is mostly relevant to subtables. I want to keep all overhead data used by the manager that is not relevant to the game reading a single entity's data, out of the way. It is most likely that game data is only needed on a per entity basis that this storage format would work well with. (pretty sure cdb's data is stored in the same way)
    </li>
  </ul>
</ul> 

<hr>
<h4>Goals:</h4>
<p>
  There are a minor features that should be added beyond basic functionality but my attention is needed elsewhere. Here is what FDB is lacking in in order of most prioritized to least (the first three may or may not be added soon):
<p/>
<ul>
  <li>Renaming Tables and Columns</li>
  <li>Row insertion</li>
  <li>Table Shifiting (re-ordering of Tables)</li>
  <li>Further Organization of Code</li>
  <li>Detailed Commentation</li>
  <li>Export to Index Removal (removal of indexes that store the order of rows in the dictionary in order to further simplify data)</li>
  <li>Seperators (equivalent to CDB's "seperator")</li>
  
  
</ul>


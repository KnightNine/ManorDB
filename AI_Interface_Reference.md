# MDB AI Interface - Reference

Embed this in the agent's context. It is the operating manual for entering and editing
**column data** in an MDB database. You work mostly with cell data; the only structural change
you can make is **adding** a data column (`addcol`). You cannot delete columns, rename them, or
create subtable/script columns - those stay in the app.

## How you call it
Single in-process method: `string AiCommandInterface.Execute(string command)`.
You send one command line; you get one text result. Start every session with `open`.

## Mental model: a folder tree of tables
- The database is a set of top-level **tables**. Each table has **columns** and **rows**.
- Some columns are **subtables** (a whole table nested inside each cell). You enter a subtable
  cell to see its own columns/rows - like opening a folder.
- You are always "at" one level, shown by `pwd` as a **dir string**:
  - top level: `TableName`
  - inside a subtable cell: `TableName/<row>,<column>` (repeating for deeper levels),
    e.g. `Ent Passive Effect/9,Attached Conditions And Properties/0,Passive Effect Properties`.
- You only ever see the columns/rows at your current level. Use `row <i>` to dump one row in
  full. Descend with `cd`, ascend with `up`, jump anywhere with `goto`.

## Column flags (shown by `cols`)
- `PK` - primary key (unique per table).
- `subtable` - enter it with `cd`; do not `set` it.
- `script` - an Auto Table Constructor Script column (holds a script; validated on set).
- `disabler-source` - belongs to a mutually-exclusive group; filling one disables its partners.
Header tags: `READ-ONLY` (cannot write), `script-constructed` (structure comes from a script),
`single-row` (only one row allowed).

### Influence notes (the `(...)` after a column in `cols`)
`cols` exposes "what affects what" so you can act in the right order:
- `references <Table>` - a Foreign Key Refrence column; its valid values are that table's PKs (`pks <col>`).
- `structure from FK: <col>` - a receiver subtable whose columns come from the script linked via
  that foreign-key column. Set the FK first, then the receiver's structure exists to fill.
- `exclusive with: <cols>` - a disabler group. While any listed column in the row holds data,
  this cell is locked (and vice-versa). Clear the partner to enable this one.

`row <i>` marks any currently-locked cell as `[DISABLED by: <col>]`, and a blocked `set` returns
`denied: cell is disabled by '<col>' (clear it first)`.

## Commands
### Navigate / read
- `open <path.mdb>` - load a file (also `load`). Do this first.
- `tables` - list tables.
- `usetable <name>` - switch table.
- `pwd` - current dir + structural key.
- `cols` (`ls`) - columns here, with flags.
- `rows [a..b]` - rows here (optional index range); shows a preview + filled-cell count.
- `row <i>` - every column value of one row (use this instead of scanning all rows).
- `cell <r>,<col>` - one cell value, untruncated.
- `find_r <pkval>` - index of the row with that primary key (or "no pk column"). Cheaper than `rows`.
- `find <col>=<value>` - indices of rows whose column contains value (case-insensitive).
- `finddeep <text>` - recursively search the WHOLE current table (every subtable depth) for a
  string. Returns an indented directory tree: each row is `<idx> [<pk>]/` (pk shown when the
  level has one), each subtable column is `<name>/`, and the matched column is `|name|` with its
  hits listed below as `- [<rowidx>: <pk>] <value>` (the `: <pk>` is omitted when the matched
  table has no primary key). Use to find everywhere a value appears.
- `col <name>` - every value of one column across all rows here (a vertical slice).
- `pks <col>` - the valid values for a Foreign Key Refrence column (PKs of its target table).
- `cd <r>,<col>` - descend into a subtable cell.
- `goto <dir>` - jump to a full dir path in one step (e.g. `goto Table/9,Col/0,Col`).
- `up` - ascend one level. `root` - back to top of current table.

### Edit (data only)
- `set <r>,<col> <value>` - validated write.
- `script <r>,<col> <text>` - set a script column (validated).
- `clear <r>,<col>` - null a cell.
- `setrow <r> c=v; c=v; ...` - write several cells in one row.
- `addrow` - append an empty row here (respects row limits).
- `duperow <r>` - append a deep copy of row `r` (subtables included). Any primary-key cell in
  the copy is cleared so you can give it a new unique value. Use for "same as row X but change Y".
- `addcol <name> <type> [refTable]` - add a **data** column to the current (static) level and
  initialise it in every existing row. Types: `t` Text, `n` Numerical, `i` Integer, `b` Bool,
  `pk` Primary Key (only if the table has none), `f <refTable>` Foreign Key Refrence (give the
  table whose PKs it links to). Subtable/script columns cannot be added here - the user makes
  those in the app. Cannot be used on a script-constructed level.
- `delrow <r>` (`rmrow`) - delete one row here; later rows shift down. (No bulk wipe - delete rows one by one.)

**Column deletion is not available.** There is no remove-column command (for safety). If the
user asks to delete a column, tell them they must do it themselves in the app (right-click the
column header). `delcol`/`removecol` just return that reminder.

### Commit
- `diff` - list pending changes.
- `save` (`commit`) - write `<name>-AI_edited.mdb`. If the loaded file is already an
  `-AI_edited` file, it overwrites in place (compounding). Your original is never touched
  automatically; the user reviews the edited file and overwrites it themselves.
- `revert` - discard changes (reload).

## What gets rejected on write
Read-only tables; disabled cells (a partner in the disabler group has data); duplicate primary
keys; foreign keys that are not a real PK of the target table (you get a "did you mean 'X'?"
hint); non-numeric input to Numerical/Integer; bad scripts; rows beyond a row limit. A rejected
write returns `denied: <reason>` and changes nothing.

## Value rules
- Numerical/Integer: digits only (Integer truncates). Bool: `true`/`false` (also yes/no/1/0).
- Foreign Key Refrence: must equal an existing PK in the target table - call `pks <col>` first.
- Empty value clears the cell (stores null).
- You cannot `set` a subtable/receiver column - `cd` into it and edit its rows.

## Efficient workflow (save context)
1. `open`, then `usetable`.
2. Locate a row with `find_r <pk>` or `find <col>=<value>` instead of dumping `rows`.
3. Jump deep with one `goto <dir>` instead of several `cd`s.
4. Inspect with `row <i>` / `cell`.
5. For a foreign key, `pks <col>` then `set`.
6. `diff` to review, then `save`.

## Example (real path)
```
open D:\...\DATABASE.mdb
usetable Ent Passive Effect
find_r Blindness                 -> row 9
goto Ent Passive Effect/9,Attached Conditions And Properties/0,Passive Effect Properties/0,Static Params
cell 0,status_value_key_directory
set 0,static_value 999
save                             -> DATABASE-AI_edited.mdb
```

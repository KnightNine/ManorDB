using System;
using System.Collections.Generic;
using System.IO;

namespace MDB.AIInterface
{
    // Headless navigation state for the AI command layer.
    // The model itself lives in the shared DatabaseFunct.currentData / selectedTable;
    // this only tracks which file is loaded and the current depth (Cwd, a dir string).
    internal class AiSession
    {
        // path of the .mdb currently loaded, or null
        public string LoadedPath;
        // dir string, e.g. "MyTable" (top level) or "MyTable/3,colA" (inside a subtable cell)
        public string Cwd;
        // human-readable log of edits made since load/revert
        public List<string> ChangeLog = new List<string>();

        public void Log(string entry) { ChangeLog.Add(entry); }

        // load an .mdb headlessly into the model; Cwd -> first main table
        public string Open(string mdbPath)
        {
            if (string.IsNullOrWhiteSpace(mdbPath))
                return "usage: open <path.mdb>";
            if (!File.Exists(mdbPath))
                return "error: file not found: " + mdbPath;

            int largestATCS;
            string err;
            var cd = InputOutput.DeserializeMDB(mdbPath, out largestATCS, out err);
            if (cd == null)
                return "error: could not load file: " + err;

            DatabaseFunct.currentData = cd;
            // headless: keep this set so reused model paths suppress their UI MessageBoxes
            // (most are gated by !loadingTable) and skip grid work
            DatabaseFunct.loadingTable = true;
            // register any script-column-type duplicates this file uses, so those
            // column types resolve correctly during rendering/validation
            if (largestATCS > ColumnTypes.scriptColumnTypeDuplicates)
                ColumnTypes.SetScriptColumnTypeDuplicates(largestATCS);

            string[] mains = DatabaseFunct.GetMainTableKeys();
            if (mains.Length == 0)
                return "error: file has no tables";

            DatabaseFunct.selectedTable = mains[0];
            Cwd = mains[0];
            LoadedPath = mdbPath;
            ChangeLog.Clear();
            return "loaded " + Path.GetFileName(mdbPath) + "; current table: " + mains[0]
                   + " (" + mains.Length + " table(s))";
        }

        // write the edited model to "<name>-AI_edited.mdb"; if the loaded file already is an
        // -AI_edited file, overwrite it in place (a compounding change)
        public string Save()
        {
            if (LoadedPath == null) return "error: no file loaded";

            string dir = Path.GetDirectoryName(LoadedPath);
            string name = Path.GetFileNameWithoutExtension(LoadedPath);
            string target = name.Contains("-AI_edited")
                ? LoadedPath
                : Path.Combine(dir, name + "-AI_edited.mdb");

            string js = Newtonsoft.Json.JsonConvert.SerializeObject(DatabaseFunct.currentData);
            File.WriteAllText(target, js);

            bool compounding = (target == LoadedPath);
            LoadedPath = target; // further saves compound onto the edited file
            return (compounding ? "overwrote " : "saved ") + target
                   + " (" + ChangeLog.Count + " change(s))";
        }

        // discard unsaved edits by reloading the current file
        public string Revert()
        {
            if (LoadedPath == null) return "error: no file loaded";
            string path = LoadedPath;
            string r = Open(path);
            return r.StartsWith("error") ? r : "reverted to " + Path.GetFileName(path);
        }
    }
}

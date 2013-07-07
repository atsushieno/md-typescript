//
// Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

module Harness {

    // Assert functions
    export module Assert {
        import assert = Harness.Assert;
        export var bugIds: string[] = [];
        export var throwAssertError = (error: Error) => {
            throw error;
        };

        // Marks that the current scenario is impacted by a bug
        export function bug(id: string) {
            if (bugIds.indexOf(id) < 0) {
                bugIds.push(id);
            }
        }

        // If there are any bugs in the test code, mark the scenario as impacted appropriately
        export function bugs(content: string) {
            var bugs = content.match(/\bbug (\d+)/i);
            if (bugs) {
                bugs.forEach(bug => assert.bug(bug));
            }
        }

        export function is(result: boolean, msg?: string) {
            if (!result) {
                throwAssertError(new Error(msg || "Expected true, got false."));
            }
        }

        export function arrayLengthIs(arr: any[], length: number) {
            if (arr.length != length) {
                var actual = '';
                arr.forEach(n => actual = actual + '\n      ' + n.toString());
                throwAssertError(new Error('Expected array to have ' + length + ' elements. Found ' + arr.length + '. Actual elements were:' + actual));
            }
        }

        export function equal(actual, expected) {
            if (actual !== expected) {
                throwAssertError(new Error("Expected " + actual + " to equal " + expected));
            }
        }

        export function notEqual(actual, expected) {
            if (actual === expected) {
                throwAssertError(new Error("Expected " + actual + " to *not* equal " + expected));
            }
        }

        export function notNull(result) {
            if (result === null) {
                throwAssertError(new Error("Expected " + result + " to *not* be null"));
            }
        }

        export function noDiff(text1: string, text2: string) {
            text1 = text1.replace(/^\s+|\s+$/g, "").replace(/\r\n?/g, "\n");
            text2 = text2.replace(/^\s+|\s+$/g, "").replace(/\r\n?/g, "\n");

            if (text1 !== text2) {
                var errorString = "";
                var text1Lines = text1.split(/\n/);
                var text2Lines = text2.split(/\n/);
                for (var i = 0; i < text1Lines.length; i++) {
                    if (text1Lines[i] !== text2Lines[i]) {
                        errorString += "Difference at line " + (i + 1) + ":\n";
                        errorString += "                  Left File: " + text1Lines[i] + "\n";
                        errorString += "                 Right File: " + text2Lines[i] + "\n\n";
                    }
                }
                throwAssertError(new Error(errorString));
            }
        }

        export function arrayContains(arr: any[], contains: any[]) {
            var found;

            for (var i = 0; i < contains.length; i++) {
                found = false;

                for (var j = 0; j < arr.length; j++) {
                    if (arr[j] === contains[i]) {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    throwAssertError(new Error("Expected array to contain \"" + contains[i] + "\""));
                }
            }
        }

        export function arrayContainsOnce(arr: any[], filter: (item: any) => boolean) {
            var foundCount = 0;

            for (var i = 0; i < arr.length; i++) {
                if (filter(arr[i])) {
                    foundCount++;
                }
            }

            if (foundCount !== 1) {
                throwAssertError(new Error("Expected array to match element only once (instead of " + foundCount + " times)"));
            }
        }
    }

    import assert = Harness.Assert;

    /** Splits the given string on \r\n or on only \n if that fails */
    export function splitContentByNewlines(content: string) {
        // Split up the input file by line
        // Note: IE JS engine incorrectly handles consecutive delimiters here when using RegExp split, so
        // we have to string-based splitting instead and try to figure out the delimiting chars
        var lines = content.split('\r\n');
        if (lines.length === 1) {
            lines = content.split('\n');
        }
        return lines;
    }

    export class ScriptInfo {
        public version: number = 1;
        public editRanges: { length: number; textChangeRange: TypeScript.TextChangeRange; }[] = [];
        public lineMap: TypeScript.LineMap = null;

        constructor(public fileName: string, public content: string, public isOpen = true, public byteOrderMark: ByteOrderMark = ByteOrderMark.None) {
            this.setContent(content);
        }

        private setContent(content: string): void {
            this.content = content;
            this.lineMap = TypeScript.LineMap.fromString(content);
        }

        public updateContent(content: string): void {
            this.editRanges = [];
            this.setContent(content);
            this.version++;
        }

        public editContent(minChar: number, limChar: number, newText: string): void {
            // Apply edits
            var prefix = this.content.substring(0, minChar);
            var middle = newText;
            var suffix = this.content.substring(limChar);
            this.setContent(prefix + middle + suffix);

            // Store edit range + new length of script
            this.editRanges.push({
                length: this.content.length,
                textChangeRange: new TypeScript.TextChangeRange(
                    TypeScript.TextSpan.fromBounds(minChar, limChar), newText.length)
            });

            // Update version #
            this.version++;
        }

        public getTextChangeRangeBetweenVersions(startVersion: number, endVersion: number): TypeScript.TextChangeRange {
            if (startVersion === endVersion) {
                // No edits!
                return TypeScript.TextChangeRange.unchanged;
            }

            var initialEditRangeIndex = this.editRanges.length - (this.version - startVersion);
            var lastEditRangeIndex = this.editRanges.length - (this.version - endVersion);

            var entries = this.editRanges.slice(initialEditRangeIndex, lastEditRangeIndex);
            return TypeScript.TextChangeRange.collapseChangesAcrossMultipleVersions(entries.map(e => e.textChangeRange));
        }
    }

    class ScriptSnapshotShim implements Services.IScriptSnapshotShim {
        private lineMap: TypeScript.LineMap = null;
        private textSnapshot: string;
        private version: number;

        constructor(private scriptInfo: ScriptInfo) {
            this.textSnapshot = scriptInfo.content;
            this.version = scriptInfo.version;
        }

        public getText(start: number, end: number): string {
            return this.textSnapshot.substring(start, end);
        }

        public getLength(): number {
            return this.textSnapshot.length;
        }

        public getLineStartPositions(): string {
            if (this.lineMap === null) {
                this.lineMap = TypeScript.LineMap.fromString(this.textSnapshot);
            }

            return JSON.stringify(this.lineMap.lineStarts());
        }

        public getTextChangeRangeSinceVersion(scriptVersion: number): string {
            var range = this.scriptInfo.getTextChangeRangeBetweenVersions(scriptVersion, this.version);
            if (range === null) {
                return null;
            }

            return JSON.stringify({ span: { start: range.span().start(), length: range.span().length() }, newLength: range.newLength() });
        }
    }

    export class TypeScriptLS implements Services.ILanguageServiceShimHost {
        private ls: Services.ILanguageServiceShim = null;

        private fileNameToScript = new TypeScript.StringHashTable();

		/*
        public addDefaultLibrary() {
            this.addScript("lib.d.ts", Harness.Compiler.libText);
        }

        public addFile(fileName: string) {
            var code = readFile(fileName).contents;
            this.addScript(fileName, code);
        }
        */

        private getScriptInfo(fileName: string): ScriptInfo {
            return this.fileNameToScript.lookup(fileName);
        }

        public addScript(fileName: string, content: string) {
            var script = new ScriptInfo(fileName, content);
            this.fileNameToScript.add(fileName, script);
        }

        public updateScript(fileName: string, content: string) {
            var script = this.getScriptInfo(fileName);
            if (script !== null) {
                script.updateContent(content);
                return;
            }

            this.addScript(fileName, content);
        }

        public editScript(fileName: string, minChar: number, limChar: number, newText: string) {
            var script = this.getScriptInfo(fileName);
            if (script !== null) {
                script.editContent(minChar, limChar, newText);
                return;
            }

            throw new Error("No script with name '" + fileName + "'");
        }

        //////////////////////////////////////////////////////////////////////
        // ILogger implementation
        //
        public information(): boolean { return false; }
        public debug(): boolean { return true; }
        public warning(): boolean { return true; }
        public error(): boolean { return true; }
        public fatal(): boolean { return true; }

        public log(s: string): void {
            // For debugging...
            //IO.printLine("TypeScriptLS:" + s);
        }

        //////////////////////////////////////////////////////////////////////
        // ILanguageServiceShimHost implementation
        //

        public getCompilationSettings(): string/*json for Tools.CompilationSettings*/ {
            return ""; // i.e. default settings
        }

        public getScriptFileNames(): string {
            return JSON.stringify(this.fileNameToScript.getAllKeys());
        }

        public getScriptSnapshot(fileName: string): Services.IScriptSnapshotShim {
            return new ScriptSnapshotShim(this.getScriptInfo(fileName));
        }

        public getScriptVersion(fileName: string): number {
            return this.getScriptInfo(fileName).version;
        }

        public getScriptIsOpen(fileName: string): boolean {
            return this.getScriptInfo(fileName).isOpen;
        }

        public getScriptByteOrderMark(fileName: string): ByteOrderMark {
            return this.getScriptInfo(fileName).byteOrderMark;
        }

        public getDiagnosticsObject(): Services.ILanguageServicesDiagnostics {
            return new LanguageServicesDiagnostics("");
        }

        public fileExists(s: string) {
            return IO.fileExists(s);
        }

        public directoryExists(s: string) {
            return IO.directoryExists(s);
        }

        public resolveRelativePath(path: string, directory: string): string {
            if (TypeScript.isRooted(path) || !directory) {
                return IO.resolvePath(path);
            }
            else {
                return IO.resolvePath(IOUtils.combine(directory, path));
            }
        }

        public getParentDirectory(path: string): string {
            return IO.dirName(path);
        }

        /** Return a new instance of the language service shim, up-to-date wrt to typecheck.
         *  To access the non-shim (i.e. actual) language service, use the "ls.languageService" property.
         */
        public getLanguageService(): Services.ILanguageServiceShim {
            var ls = new Services.TypeScriptServicesFactory().createLanguageServiceShim(this);
            ls.refresh(true);
            this.ls = ls;
            return ls;
        }

        /** Parse file given its source text */
        public parseSourceText(fileName: string, sourceText: TypeScript.IScriptSnapshot): TypeScript.Script {
            var compilationSettings = new TypeScript.CompilationSettings();
            var parseOptions = TypeScript.getParseOptions(compilationSettings);
            return TypeScript.SyntaxTreeToAstVisitor.visit(
                TypeScript.Parser.parse(fileName, TypeScript.SimpleText.fromScriptSnapshot(sourceText), TypeScript.isDTSFile(fileName), TypeScript.LanguageVersion.EcmaScript5, parseOptions),
                fileName, compilationSettings, /*incrementalAST: */ true);
        }

        /** Parse a file on disk given its fileName */
        public parseFile(fileName: string) {
            var sourceText = TypeScript.ScriptSnapshot.fromString(IO.readFile(fileName).contents)
            return this.parseSourceText(fileName, sourceText);
        }

        /**
         * @param line 1 based index
         * @param col 1 based index
        */
        public lineColToPosition(fileName: string, line: number, col: number): number {
            var script: ScriptInfo = this.fileNameToScript.lookup(fileName);
            assert.notNull(script);
            assert.is(line >= 1);
            assert.is(col >= 1);

            return script.lineMap.getPosition(line - 1, col - 1);
        }

        /**
         * @param line 0 based index
         * @param col 0 based index
        */
        public positionToZeroBasedLineCol(fileName: string, position: number): TypeScript.ILineAndCharacter {
            var script: ScriptInfo = this.fileNameToScript.lookup(fileName);
            assert.notNull(script);

            var result = script.lineMap.getLineAndCharacterFromPosition(position);

            assert.is(result.line() >= 0);
            assert.is(result.character() >= 0);
            return { line: result.line(), character: result.character() };
        }

        /** Verify that applying edits to sourceFileName result in the content of the file baselineFileName */
        /*
        public checkEdits(sourceFileName: string, baselineFileName: string, edits: Services.TextEdit[]) {
            var script = readFile(sourceFileName);
            var formattedScript = this.applyEdits(script.contents, edits);
            var baseline = readFile(baselineFileName).contents;

            assert.noDiff(formattedScript, baseline);
            assert.equal(formattedScript, baseline);
        }
        */


        /** Apply an array of text edits to a string, and return the resulting string. */
        public applyEdits(content: string, edits: Services.TextEdit[]): string {
            var result = content;
            edits = this.normalizeEdits(edits);

            for (var i = edits.length - 1; i >= 0; i--) {
                var edit = edits[i];
                var prefix = result.substring(0, edit.minChar);
                var middle = edit.text;
                var suffix = result.substring(edit.limChar);
                result = prefix + middle + suffix;
            }
            return result;
        }

        /** Normalize an array of edits by removing overlapping entries and sorting entries on the minChar position. */
        private normalizeEdits(edits: Services.TextEdit[]): Services.TextEdit[] {
            var result: Services.TextEdit[] = [];

            function mapEdits(edits: Services.TextEdit[]): { edit: Services.TextEdit; index: number; }[] {
                var result = [];
                for (var i = 0; i < edits.length; i++) {
                    result.push({ edit: edits[i], index: i });
                }
                return result;
            }

            var temp = mapEdits(edits).sort(function (a, b) {
                var result = a.edit.minChar - b.edit.minChar;
                if (result === 0)
                    result = a.index - b.index;
                return result;
            });

            var current = 0;
            var next = 1;
            while (current < temp.length) {
                var currentEdit = temp[current].edit;

                // Last edit
                if (next >= temp.length) {
                    result.push(currentEdit);
                    current++;
                    continue;
                }
                var nextEdit = temp[next].edit;

                var gap = nextEdit.minChar - currentEdit.limChar;

                // non-overlapping edits
                if (gap >= 0) {
                    result.push(currentEdit);
                    current = next;
                    next++;
                    continue;
                }

                // overlapping edits: for now, we only support ignoring an next edit 
                // entirely contained in the current edit.
                if (currentEdit.limChar >= nextEdit.limChar) {
                    next++;
                    continue;
                }
                else {
                    throw new Error("Trying to apply overlapping edits");
                }
            }

            return result;
        }
    }

    export class LanguageServicesDiagnostics implements Services.ILanguageServicesDiagnostics {

        constructor(private destination: string) { }

        public log(content: string): void {
            //Imitates the LanguageServicesDiagnostics object when not in Visual Studio
        }

    }
}

mergeInto(LibraryManager.library, {
    SaveFileJS: function(fileNamePtr, jsonPtr) {
        var fileName = UTF8ToString(fileNamePtr);
        var json = UTF8ToString(jsonPtr);

        var blob = new Blob([json], { type: 'application/json' });
        var url = URL.createObjectURL(blob);

        var a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();

        URL.revokeObjectURL(url);
    }
});
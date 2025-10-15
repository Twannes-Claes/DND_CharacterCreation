mergeInto(LibraryManager.library, 
{
    SaveFileJS: function(fileNamePtr, jsonPtr) 
    {
        var filename = UTF8ToString(fileNamePtr);
    	var json = UTF8ToString(jsonPtr);

    	// Create a blob of the data
   		var blob = new Blob([json], { type: "application/json" });
    	var url = URL.createObjectURL(blob);

    	var a = document.createElement('a');
    	a.href = url;
   		a.download = filename || 'data.json';
		a.style.display = 'none';

    	document.body.appendChild(a);
    	a.click();

    	setTimeout(function () 
		{
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 100);
    }
});
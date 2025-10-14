mergeInto(LibraryManager.library, 
{
    SaveFileJS: function(fileNamePtr, jsonPtr) 
    {
        var filename = UTF8ToString(filenamePtr);
    	var data = UTF8ToString(dataPtr);

    	// Create a blob of the data
   	var blob = new Blob([data], { type: "application/json" });
    	var url = URL.createObjectURL(blob);

    	// Create a temporary <a> element to trigger download
    	var a = document.createElement('a');
    	a.href = url;
   	a.download = filename;

    	document.body.appendChild(a);
    	a.click();

    	setTimeout(function () 
	{
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 100);
    }
});
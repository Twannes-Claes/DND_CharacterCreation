mergeInto(LibraryManager.library, 
{
    UploadFileJS: function(gameObjectNamePtr, callbackMethodPtr) 
	{
        var gameObjectName = UTF8ToString(gameObjectNamePtr);
        var callbackMethod = UTF8ToString(callbackMethodPtr);

        // Create hidden file input element
        var input = document.createElement('input');
        input.type = 'file';
        input.accept = '.json,application/json';
        input.style.display = 'none';

        input.onchange = function(event) 
		{
            var file = event.target.files[0];
            if (!file) return;

            var reader = new FileReader();

            reader.onload = function(e) 
			{
                var fileContent = e.target.result;
                // Send the text content back to Unity
                SendMessage(gameObjectName, callbackMethod, fileContent);
            };

            reader.readAsText(file);
        };

        document.body.appendChild(input);
        input.click();

        setTimeout(() => document.body.removeChild(input), 1000);
    }
});
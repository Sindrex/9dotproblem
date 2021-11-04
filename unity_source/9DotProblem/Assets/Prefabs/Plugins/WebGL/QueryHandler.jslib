var QueryHandler = {
    GetURL: function(){
        var url = window.top.location.href;
        console.log("Plugin: url found: " + url);

        var returnStr = url;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    OpenURL: function(url){
      url = Pointer_stringify(url);
      console.log('Opening link: ' + url);
      window.open(url,'_blank');
    },
    CheckVisible: function(){
      if (document.visibilityState === 'visible') {
        console.log('Plugin: Tab is visible');
        return true;
      } else {
        console.log('Plugin: Tab is NOT visible');
        return false;
      }
    }
};
mergeInto(LibraryManager.library, QueryHandler);

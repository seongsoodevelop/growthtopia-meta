mergeInto(LibraryManager.library, {
    GetTicketTokenFromReact: function() {
        try {
            window.dispatchReactUnityEvent("GetTicketTokenFromReact");
        }
        catch(e) {

        }
    }
})
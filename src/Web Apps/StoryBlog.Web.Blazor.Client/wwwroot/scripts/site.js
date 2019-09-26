(function(document, window) {
    window.toggleBody = function (toggle) {
        var element = document.getElementsByTagName('body')[0];
        element.className = !!toggle ? 'modal-open' : null;
    };

    function createEditor (config) {
        var editor = {};

        editor.getContent = function(hostId) {
            var hostFrame = document.getElementById(hostId);
            if (!!hostFrame) {
                var doc = hostFrame.contentDocument || hostFrame.contentWindow.document;
                return doc.body.innerHTML;
            }
            return null;
        };

        editor.setContent = function(hostId, content) {
            var hostFrame = document.getElementById(hostId);
            if (!!hostFrame) {
                var doc = hostFrame.contentDocument || hostFrame.contentWindow.document;
                doc.body.setAttribute('contenteditable', true);
                doc.body.innerHTML = content;
            }
        };

        return editor;
    };

    window.contentEditor = createEditor(null);

})(document, window);
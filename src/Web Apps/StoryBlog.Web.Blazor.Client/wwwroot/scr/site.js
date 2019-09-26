(function(document, window) {
    window.toggleBody = function (toggle) {
        var element = document.getElementsByTagName('body')[0];
        element.className = !!toggle ? 'modal-open' : null;
    };

    window.getContent = function(host) {
        return document.getElementById(host).contentWindow.document.innerHTML.toString();
    };

    window.setContent = function(hostId, content) {
        var hostFrame = document.getElementById(hostId);
        if (!!hostFrame) {
            var doc = hostFrame.contentDocument || hostFrame.contentWindow.document;
            doc.body.setAttribute('contenteditable', true);
            doc.body.innerHTML = content;
        }
    };
})(document, window);
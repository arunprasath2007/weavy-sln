var Dop = Dop || {};

Dop.SearchResults = function () {
    function hideAll() {
        $('.search-result-item').hide();
    }
    function show(type, event) {
        hideAll();
        $('.search-result-item[name=' + type + ']').show();
        setActiveLink(event);
        return false;
    }
    function showAll(event) {
        $('.search-result-item').show();
        setActiveLink(event);
        return false;
    }
    function setActiveLink(event) {
        $('.nav-link').removeClass('active');
        $(event.target).addClass('active');
    }

    return {
        hideAll,
        showAll,
        show
    }
}();
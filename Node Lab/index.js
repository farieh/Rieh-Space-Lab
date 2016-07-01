
const electron = require('electron');
const {app} = electron;
const windowManager = require('electron-window-manager');

app.on('ready', function () {
    windowManager.init({
        'onLoadFailure': function (window) {
            window.loadURL('/static/404.html');
        }
    });
    windowManager.setDefaultSetup({
        'width': 600,
        'height': 450,
        'position': 'right'
    });

    // Open a window
    var homeWindow = windowManager.open(
        'home',
        'Welcome ...',
        '/static/dashboard.html',
        false,
        {
            'width': 600,
            'height': 450,
            'showDevTools': false,
            'resizable': false
        }
    );

    

});

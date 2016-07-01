const electron = require('electron');
const {app} = electron;
//const windowManager = require('electron-window-manager');


function createNew() {
    var crtNew = windowManager.createNew(
        'home2',
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
}
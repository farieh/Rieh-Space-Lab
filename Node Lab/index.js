
const electron = require('electron');
const {app} = electron;
// Module to create native browser window.
const {BrowserWindow} = electron;

function createWindow() {
    // Create the browser window.
    win = new BrowserWindow({ width: 400, height: 400, frame:false });
    win.setMenu(null);
    win.setResizable(false);


    // and load the index.html of the app.
    win.loadURL(`file://${__dirname}/static/dashboard.html`);

    // Open the DevTools.
    //win.webContents.openDevTools();

    // Emitted when the window is closed.
    win.on('closed', () => {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        win = null;
    });
}

app.on('ready', function () {
    createWindow();
});

const ipc = require('electron').ipcMain
const dialog = require('electron').dialog

ipc.on('open-information-dialog', function (event) {
    const options = {
        type: 'info',
        title: 'Information',
        message: "This is an information dialog. Isn't it nice?",
        buttons: ['Yes', 'No']
    }
    dialog.showMessageBox(options, function (index) {
        event.sender.send('information-dialog-selection', index)
    })
})



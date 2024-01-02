/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
    config.language = 'vi';
    //config.defaultLanguage = 'vi';
    // config.uiColor = '#AADC6E';
    config.removePlugins = 'easyimage, cloudservices';
    config.extraPlugins = "lineheight,table,tabletools,tableresize,tableselection";
    config.line_height = "1;1.25;1.5;1.75;2;2.25;2.5;2.75;3;3.25;3.5;3.75;4;4.25;4.5;4.75;5";
    config.contentsCss = "/css/fonts.css";
    //config.font_names = " Myriad Pro Regular;" + CKEDITOR.config.font_names;    
};
CKEDITOR.on('dialogDefinition', function (ev) {
    // Take the dialog name and its definition from the event data.
    var dialogName = ev.data.name;
    var dialogDefinition = ev.data.definition;
    // Check if the definition is from the dialog we're
    // interested in (the 'image' dialog).
    if (dialogName == 'image') {
        // Get a reference to the 'Image Info' tab.
        var infoTab = dialogDefinition.getContents('info');
        // Remove unnecessary widgets/elements from the 'Image Info' tab.        
        infoTab.remove('txtHSpace');
        infoTab.remove('txtVSpace');
        infoTab.remove('txtBorder');
        infoTab.remove('txtAlt');
        //infoTab.remove('txtWidth');
        //infoTab.remove('txtHeight');
        infoTab.remove('htmlPreview');
        infoTab.remove('cmbAlign');
        infoTab.remove('ratioLock');
    }
});

var map = new ol.Map({
    target: 'map', // <-- This is the id of the div in which the map will be built.
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        })
    ],

    view: new ol.View({
        center: ol.proj.fromLonLat([7.0985774, 43.6365619]), // <-- Those are the GPS coordinates to center the map to.
        zoom: 10 // You can adjust the default zoom.
    })

});


// Create an array containing the GPS positions you want to draw
var coords = [[7.0985774, 43.6365619], [7.1682519, 43.67163]];
var lineString = new ol.geom.LineString(coords);

// Transform to EPSG:3857
lineString.transform('EPSG:4326', 'EPSG:3857');

// Create the feature
var feature = new ol.Feature({
    geometry: lineString,
    name: 'Line'
});

// Configure the style of the line
var lineStyle = new ol.style.Style({
    stroke: new ol.style.Stroke({
        color: '#ffcc33',
        width: 10
    })
});

var source = new ol.source.Vector({
    features: [feature]
});

var vector = new ol.layer.Vector({
    source: source,
    style: [lineStyle]
});

map.addLayer(vector);
// import * as THREE from "https://cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js";
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
// import { GLTFLoader } from 'https://cdn.jsdelivr.net/npm/three/examples/jsm/loaders/GLTFLoader.js'

// hammer model thanks to https://www.turbosquid.com/3d-models/3d-hammer-pbr-model-1344861
// const gltfs = ['hammer.glb'];
const gltfs = ['./assets/hammedr.glb'];
let flock = new THREE.Group(); 
let flockSize = 12;

var scene = new THREE.Scene();


// function flow
setupFlock();


  // Create a basic perspective camera
  var camera = new THREE.PerspectiveCamera( 75, window.innerWidth/window.innerHeight, 0.1, 1000 );
  camera.position.z = 4;

  // Create a renderer with Antialiasing
  var renderer = new THREE.WebGLRenderer({antialias:true});

  // Configure renderer clear color
  renderer.setClearColor("#000000");

  // Configure renderer size
  renderer.setSize( window.innerWidth, window.innerHeight );

  // Append Renderer to DOM
  document.body.appendChild( renderer.domElement ); 

// init all the hammers
function setupFlock() {

  new GLTFLoader().load(
    gltfs[0],
    (gltf) => {
        flock.add(gltf.scene);
        })
  // new GLTFLoader().load(gltfs[0], function (gltf) {
  //   for (let i = 0; i < flockSize; i++) {
  //     flock.add(gltf.scene);
  //   }
  // });
  
}
render();

// Render Loop
function render (){
  requestAnimationFrame( render );

  // cube.rotation.x += 0.01;
  // cube.rotation.y += 0.01;

  // Render the scene
  renderer.render(scene, camera);
};


// Create a Cube Mesh with basic material
// var geometry = new THREE.BoxGeometry( 1, 1, 1 );
// var material = new THREE.MeshBasicMaterial( { color: "#433F81" } );
// var cube = new THREE.Mesh( geometry, material );

// // Add cube to Scene
// scene.add( cube );
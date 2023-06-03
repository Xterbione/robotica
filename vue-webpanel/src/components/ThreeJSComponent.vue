<template>
  <div class="row text-center justify-items-center">
    <div class="canvasContainer" ref="canvasContainer">
      <h1>3D vision</h1>
    </div>
  </div>
</template>

<script>
import * as THREE from 'three';
import { PLYLoader } from 'three/examples/jsm/loaders/PLYLoader.js'; // Import the PLYLoader

export default {
  mounted() {
    // Create a scene, camera, and renderer
    this.scene = new THREE.Scene();
    this.camera = new THREE.PerspectiveCamera(
      75,
      (window.innerWidth/2) / 800,
      0.1,
      1000
    );
    this.renderer = new THREE.WebGLRenderer({ antialias: true });
    this.renderer.setSize((window.innerWidth/2), 800);
    this.$refs.canvasContainer.appendChild(this.renderer.domElement);

    // Load the PLI file using PLYLoader
    const loader = new PLYLoader();
      loader.load('C:/Users/bryan/Downloads/out.ply', (geometry) => {
      const material = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
      this.pliMesh = new THREE.Mesh(geometry, material);
      this.scene.add(this.pliMesh);

      // Position the camera
      this.camera.position.z = 5;

      // Start the animation loop
      this.animate();
    });
  },
  methods: {
    animate() {
      requestAnimationFrame(this.animate);

      // Rotate the PLI mesh
      if (this.pliMesh) {
        this.pliMesh.rotation.x += 0.01;
        this.pliMesh.rotation.y += 0.01;
      }

      // Render the scene with the camera
      this.renderer.render(this.scene, this.camera);
    }
  },
  beforeUnmount() {
    // Clean up resources
    this.$refs.canvasContainer.removeChild(this.renderer.domElement);
    this.renderer.dispose();
  }
};
</script>

<style>
.canvasContainer {
  width: 200px;
}
</style>
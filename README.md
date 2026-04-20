# Tech_artist_proj_Vishal_Yadav

## 🚀 Project Overview
This project transforms a basic car prototype into a highly responsive, "juicy" arcade experience. The visual target focuses on a vibrant "Sunrise in a Desert" aesthetic with high-key lighting, combined with rigorous technical optimizations to ensure a rock-solid 60 FPS performance on low-end mobile devices.

## ⏱️ Time Tracked
| Phase     | Task                                     | Time      |
| :-------- | :--------------------------------------- | :-------- |
| Planning  | Initial Style Research                   | 0.5h      |
| Setup     | Download, Install & Project Initialize   | 0.5h      |
| Phase 1   | Environment & Post-Processing Setup      | 2.0h      |
| Phase 1   | Environment Detailing (Mountains, Sidewalk, Fence) | 0.75h     |
| Phase 1   | Asset Prep (Kenney Car & Vertex Material Setup) | 0.5h      |
| Phase 2   | Game Feel (Camera Damping)               | 0.5h      |
| Phase 2   | Procedural Car Tilt                      | 0.5h      |
| Phase 2   | Swerve Mechanics (Y-axis Weighted Drift) | 0.5h      |
| Phase 4   | VFX Motion Trails                        | 0.5h      |
| Phase 3   | Foliage Addition (Trees, Cacti)          | 0.5h      |
| Phase 3   | Foliage Pooling & GPU Instancing Tools   | 0.5h      |
| Phase 3   | Android Build Setup & Performance Testing| 0.5h      |
| **Total** |                                          | **7.75h** |

## 🛠️ Assets & Tools Used
- **Engine**: Unity 6000.3.9f1 (URP)
- **Design Tools**: Figma (for textures and UI), Blender (for models).
- **3D Assets**: Car model sourced from Kenney (CC0).
- **Task Tracking & Documentation**: Obsidian / Antigravity AI

## ⚡ Optimization & Decisions
- **Decision 1**: Focusing on **Baked Lighting** to minimize per-pixel lighting costs on mobile GPUs.
- **Decision 2**: Using **Reference-Based Design** (Dashy Crashy) to ensure a cohesive, non-realistic stylized look.
- **Decision 3**: **Full-Pipeline Vertex Color Optimization**: The entire environment (mountains, sidewalks, fences, road, desert ground) and the dynamic Player Car mesh are explicitly modeled and colored using vertex painting in Blender. To support this natively in URP, I designed a **Custom Shader Graph** constructed specifically to read and output vertex colors, creating a unified material that completely bypasses texture lookups. This saves massive amounts of texture memory and minimizes GPU sampling latency on constrained mobile devices.
- **Decision 4**: **Post-Processing (URP)**: Added **Bloom** (for sun bleed), **Vignette** (for focus), and **ACES Tonemapping** (for cinematic color response). This achieves the "High-Key" arcade look required by the brief.
- **Decision 5**: **Cinemachine Damping**: Repurposed Cinemachine's Position and Rotation Composers to leverage X/Y damping. This naturally adds a sweeping, high-speed lag sensation to lane switches with zero extra scripting overhead.
- **Decision 6**: **Procedural Car Tilt**: Implemented programmatic Y-axis rotation based on lateral velocity in `CarController.cs` to add visual weight, emphasizing cornering forces without expensive physics simulations.
- **Decision 7**: **Procedural Motion Trails**: Designed a procedural Speed Lines controller utilizing Unity's built-in Particle System (stretch render mode) to emphasize speed. This provides a strong "anime-dash" visual without relying on expensive fullscreen motion blur shaders.
- **Decision 8**: **Object Pooling vs. Static Batching**: To manage the large number of environment assets (cactus, trees, foliage), an explicit Object Pooling strategy was utilized via the `SpawnerLoop` component. Because the game design features a moving environment with a stationary player car, **Static Batching was intentionally omitted**—we cannot mark fences, trees, or cacti as `Static`. Instead, performance relies entirely on the URP SRP Batcher and GPU Instancing. To ensure minimal draw calls, I created a custom Unity Editor tool (`OptimizationAuditWindow.cs`) that scans the project to automatically validate and enable GPU Instancing across all materials, alongside a mesh vertex-count audit.
- **Decision 9**: **AI Workflow Integration**: Leveraged Antigravity AI to accelerate boilerplate scripting, maintain persistent Markdown documentation, manage task progression, and source creative inspiration, maximizing the time spent strictly on high-level technical and artistic execution.
- **Performance Benchmark (2026-04-20)**: Successfully tested the Android compiled build on a **Realme 3 (3GB RAM)**. The game maintains a **rock-solid 60 FPS**. This proves that the combination of vertex-color shading, disabled shadows on foliage, heavy object pooling, and SRP Batching successfully eliminates mobile GPU/CPU bottlenecks on low-end devices.
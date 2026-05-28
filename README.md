# TrappingGame

> A 2D precision platformer with game-feel-focused movement — built with Unity 6 & C#

![Unity](https://img.shields.io/badge/Unity-6-black?logo=unity)
![Language](https://img.shields.io/badge/C%23-2D%20Physics-239120?logo=csharp)
![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20WebGL-blue)
![Status](https://img.shields.io/badge/Status-Playable-brightgreen)

**[▶ Play in Browser](https://phuc4d.itch.io/trappingame)** · **[View Source](https://github.com/Phuc4D/TrappingGame)**

---

## 📸 Gameplay

> 🎬 *GIF coming soon — record with ScreenToGif and drop here*

---

## 🎮 About

TrappingGame is a 2D precision platformer focused on responsive, game-feel-driven movement. The player navigates trap-filled levels using a controller that implements the same leniency techniques found in production platformers like Celeste and Hollow Knight.

---

## 🏗️ Architecture Highlights

### Game-Feel Player Controller
The movement system implements four industry-standard responsiveness techniques:

| Technique | What it does |
|---|---|
| **Coyote Time** | Allows jumping for a brief window after walking off a ledge |
| **Jump Buffering** | Queues a jump input if pressed slightly before landing |
| **Wall Jump** | Launches player off walls with a directional lockout timer to prevent spam |
| **Wall Slide** | Clamps fall speed when pressing into a wall |

```csharp
// Coyote Time — player can still jump shortly after leaving ground
void HandleCoyoteTime()
{
    if (isGrounded)
        coyoteTimer = coyoteTime;
    else
        coyoteTimer -= Time.deltaTime;
}

// Jump Buffer — accepts input slightly before landing
void HandleJumpBuffer()
{
    if (Input.GetButtonDown("Jump"))
        jumpBufferTimer = jumpBufferTime;
    else
        jumpBufferTimer -= Time.deltaTime;
}
```

### Modular Obstacle System
Each obstacle is a self-contained, reusable component with no dependencies on other game systems:

| Obstacle | Behavior |
|---|---|
| `SpikeBall` | Pendulum swing with procedural chain generation at runtime |
| `FallingPlatform` | Shakes on player contact, falls, self-resets after delay |
| `Trampoline` | Physics-based bounce with animation trigger |
| `Spike` | Instant death trap |

### Checkpoint & Respawn System
- Player stores spawn point on checkpoint trigger
- Death plays pop animation + disables collider
- Respawn teleports to last checkpoint with clean physics state

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| Engine | Unity 6 |
| Language | C# |
| Physics | Unity 2D Physics |
| Version Control | Git |

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Player/         # PlayerController (movement, jump, wall)
│   ├── Obstacles/      # SpikeBall, FallingPlatform, Trampoline
│   ├── Collectibles/   # Fruits, score system
│   ├── Level/          # CheckPoint, EndFlag
│   └── Managers/       # GameManager, ScoreManager
└── Prefabs/
```

---

## 🚀 Getting Started

```bash
git clone https://github.com/Phuc4D/TrappingGame.git
# Open in Unity Hub — requires Unity 6.x
```

Or **[play directly in browser](https://phuc4d.itch.io/trappingame)** — no download needed.

---

## 👤 Author

**Nguyen Phuoc Hong Phuc**
[github.com/Phuc4D](https://github.com/Phuc4D) · [nphphucdev@gmail.com](mailto:nphphucdev@gmail.com)

# RPG Game

## 1. Core (✅ Done)
- **Movement** (✅ Done)  
- **Combat System** (✅ Done)  
- **Controls** (✅ Done)  
  - Player Inputs  
  - Enemy AI  
- **Attributes** (✅ Done)  
- **Scene Management** (✅ Done)  
- **Save and Load** (✅ Done)  
- **Stats** (✅ Done)  
- **UI** (✅ Done)  
- **Core Utility** (✅ Done)  

## 2. Inventory  

## 3. Dialogue  
- Node-based Dialogue Editor  
- Branched Dialogue  

## 4. Quest  

## 5. Shops  

## 6. Abilities  
- Fire Arrow  
- Healing  
- Selected Area-based Damage  

---

## Movement  
- The player moves using mouse clicks on a **terrain NavMesh**.  
- There are **specific areas** the player can access.  
- If an event occurs in the game and the game is saved, the player can **continue from that point** upon loading.  

---

## Combat System  
- Multiple weapon types:  
  - **Unarmed**  
  - **Sword**  
  - **Bow and Arrow**  
  - **Fireball Attack (exclusive to the player)**  

---

## Enemy AI  
- Implemented using a **Generic Finite State Machine (FSM)**  
- AI Behaviors:  
  - **Idle**  
  - **Patrol**  
  - **Chase**  
  - **Attack**  
  - **Flee**  
  - **Agro**  
- The FSM structure allows for:  
  - **Extending AI to create boss enemies**  
  - **Creating NPC behaviors**  

---

## Game Features  
- **Sound and Particle Effects**  
- **Asynchronous Scene Loading**  
- **Observer Pattern** for event handling, implemented using:  
  - **C# Events**  
  - **Unity Events**

---

## Technical Stack  
- **URP (Universal Render Pipeline) Project**  
- **LOD (Level of Detail) Environment Optimization**  
- **Prefabs and Scriptable Objects** used effectively  

---

 ## Images and Videos


![Screenshot (1)](https://github.com/user-attachments/assets/3e8430b4-8cde-47ed-b66b-9b825f96b12a)
![Screenshot (2)](https://github.com/user-attachments/assets/2e1b914e-7912-42d9-84a4-5e8e5f1f2d35)
![Screenshot (3)](https://github.com/user-attachments/assets/730dcc7c-e15e-4058-92c2-3cbd25bd4a94)
![Screenshot (4)](https://github.com/user-attachments/assets/6eb1ac64-5662-4d2c-a935-398589e47d25)
![Screenshot (5)](https://github.com/user-attachments/assets/dee8780b-56ba-470c-8553-467c90c3234b)
![Screenshot (6)](https://github.com/user-attachments/assets/bc41622c-cf14-4ac4-9cc9-4a2324a49d5f)
![Screenshot (7)](https://github.com/user-attachments/assets/0ab334e4-eb84-4854-a233-e81eb744e430)
![Screenshot (8)](https://github.com/user-attachments/assets/2ef02202-16e3-44fe-ad50-33cf55915ee7)
![Screenshot (9)](https://github.com/user-attachments/assets/0360886e-e372-4ad2-9613-963ef1e5167c)
![Screenshot (10)](https://github.com/user-attachments/assets/57b2e772-d5b8-4f45-a3af-f9b72eef56b3)
![Screenshot (11)](https://github.com/user-attachments/assets/6856ae22-b943-4987-8cd1-edd0940f2afc)

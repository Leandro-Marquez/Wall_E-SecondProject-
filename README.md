# **Pixel Wall-E - Proyecto de Programación en Unity**  
![un robot pintando píxeles en una pizarra con un pincel, inspirado en la estética de Wall-E (2)](https://github.com/user-attachments/assets/56efdf72-c9e6-4997-ae18-ad933fa8e8ea)

## **Descripción del Proyecto**  
Pixel Wall-E es una aplicación interactiva desarrollada en **Unity** que permite crear **arte pixelado** mediante un lenguaje de programación personalizado. El programa simula un robot (Wall-E) que sigue comandos para dibujar en un **canvas cuadrado**. Los usuarios pueden escribir código en un editor de texto integrado, ejecutarlo y ver los resultados en tiempo real.  

### **Características principales**  
✅ **Editor de texto con resaltado de sintaxis** (para mejor legibilidad del código).  
✅ **Canvas dinámico** (redimensionable, con píxeles editables).  
✅ **Comandos básicos** (`DrawLine`, `DrawCircle`, `Fill`, etc.).  
✅ **Variables, funciones y saltos condicionales** (`GoTo`).  
✅ **Carga y guardado de archivos** (extensión `.pw`).  
✅ **Sistema de errores** (sintaxis, ejecución, límites del canvas).  

---

## **Instalación y Ejecución**  

### **Requisitos**  
- **Unity 2021.3 o superior** (probado en LTS).  
- **Visual Studio / Rider / VS Code** (para edición de scripts).  

### **Pasos para Ejecutar**  
1. **Clona el repositorio**:  
   ```bash
   git clone https://github.com/tu-usuario/pixel-wall-e-unity.git
   ```
2. **Abre el proyecto en Unity**:  
   - Ve a `File > Open Project` y selecciona la carpeta del proyecto.  
3. **Ejecuta desde el Editor de Unity**:  
   - Abre la escena principal (`MainScene.unity`).  
   - Presiona **Play** para probar.  

### **Build para Windows/Linux/Mac**  
1. Ve a `File > Build Settings`.  
2. Selecciona la plataforma deseada.  
3. Haz clic en **Build** y elige una carpeta de destino.  

---

## **Uso del Programa**  

### **Interfaz Gráfica**  
- **Editor de código**:  
  - Escribe o pega tus comandos en formato `.pw`.  
  - Usa el botón **"Ejecutar"** para procesar el código.  
- **Canvas**:  
  - Se actualiza en tiempo real según los comandos ejecutados.  
  - Puedes cambiar su tamaño desde la UI.  
- **Botones principales**:  
  - **🔄 Redimensionar**: Cambia el tamaño del canvas.  
  - **▶️ Ejecutar**: Corre el código escrito.  
  - **💾 Guardar**: Exporta el código a un archivo `.pw`.  
  - **📂 Cargar**: Importa un archivo `.pw` existente.  

### **Ejemplo de Código Válido**  
```plaintext
Spawn(0, 0)  
Color("Blue")  
Size(3)  
DrawLine(1, 0, 10)  
DrawCircle(1, 1, 5)  
Fill()  
```
*(Este código posiciona a Wall-E en `(0,0)`, dibuja una línea azul hacia la derecha, un círculo y rellena el área conectada).*  

---

## **Manejo de Errores**  
El programa detecta y muestra errores comunes:  
❌ **Errores de sintaxis** (comandos mal escritos).  
❌ **Ejecución inválida** (Wall-E fuera del canvas).  
❌ **Variables no definidas**.  

*(Los errores se muestran en la consola de Unity y en un panel de la UI).*  

---

## **Extensibilidad y Mejoras Futuras**  
🛠 **Posibles mejoras**:  
- **Más colores y formas** (triángulos, polígonos).  
- **Exportar imagen** (PNG/JPEG del canvas).  
- **Autocompletado de código**.  
- **Soporte para bucles (`for`, `while`)**.  

---

## **Créditos y Licencia**  
- **Desarrollado por**: [Leandro Marquez Blanco]  
- **Repositorio**: [GitHub Link]([https://github.com/tu-usuario/pixel-wall-e-unity](https://github.com/Leandro-Marquez/Wall_E-SecondProject-))  

---

### **¿Preguntas o problemas?**  
¡Siéntete libre de abrir un **issue** en el repositorio! 🚀  

--- 

🔹 **¡Diviértete creando arte con Pixel Wall-E!** 🎨🤖

﻿using System;

namespace ScpDriverInterface
{
	/// <summary>
	/// A virtual Xbox 360 Controller. After setting the desired values, use the GetReport() method to generate a controller report that can be used with ScpBus's Report() method.
	/// </summary>
	public class X360Controller
	{
		/// <summary>
		/// Generates a new X360Controller object with the default initial state (no buttons pressed, all analog inputs 0).
		/// </summary>
		public X360Controller()
		{
			Buttons = X360Buttons.None;
			LeftTrigger = 0;
			RightTrigger = 0;
			LeftStickX = 0;
			LeftStickY = 0;
			RightStickX = 0;
			RightStickY = 0;
		}
		
		/// <summary>
		/// Generates a new X360Controller object. Optionally, you can specify the initial state of the controller.
		/// </summary>
		/// <param name="buttons">The pressed buttons. Use like flags (i.e. (X360Buttons.A | X360Buttons.X) would be mean both A and X are pressed).</param>
		/// <param name="leftTrigger">Left trigger analog input. 0 to 255.</param>
		/// <param name="rightTrigger">Right trigger analog input. 0 to 255.</param>
		/// <param name="leftStickX">Left stick X-axis. -32,768 to 32,767.</param>
		/// <param name="leftStickY">Left stick Y-axis. -32,768 to 32,767.</param>
		/// <param name="rightStickX">Right stick X-axis. -32,768 to 32,767.</param>
		/// <param name="rightStickY">Right stick Y-axis. -32,768 to 32,767.</param>
		public X360Controller(X360Buttons buttons, byte leftTrigger, byte rightTrigger, short leftStickX, short leftStickY, short rightStickX, short rightStickY)
		{
			Buttons = buttons;
			LeftTrigger = leftTrigger;
			RightTrigger = rightTrigger;
			LeftStickX = leftStickX;
			LeftStickY = leftStickY;
			RightStickX = rightStickX;
			RightStickY = rightStickY;
		}

		/// <summary>
		/// Generates a new X360Controller object with the same values as the specified X360Controller object.
		/// </summary>
		/// <param name="controller">An X360Controller object to copy values from.</param>
		public X360Controller(X360Controller controller)
		{
			Buttons = controller.Buttons;
			LeftTrigger = controller.LeftTrigger;
			RightTrigger = controller.RightTrigger;
			LeftStickX = controller.LeftStickX;
			LeftStickY = controller.LeftStickY;
			RightStickX = controller.RightStickX;
			RightStickY = controller.RightStickY;
		}

        /// <summary>The controller's currently pressed buttons. Use the X360Button values like flags.</summary>
        /// <remarks>For example, (X360Buttons.A | X360Buttons.X) would be mean both A and X are pressed.</remarks>
        public X360Buttons Buttons { get; set; }

		/// <summary>
		/// The controller's left trigger analog input. Value can range from 0 to 255.
		/// </summary>
		public byte LeftTrigger { get; set; }

		/// <summary>
		/// The controller's right trigger analog input. Value can range from 0 to 255.
		/// </summary>
		public byte RightTrigger { get; set; }

		/// <summary>
		/// The controller's left stick X-axis. Value can range from -32,768 to 32,767.
		/// </summary>
		public short LeftStickX { get; set; }

		/// <summary>
		/// The controller's left stick Y-axis. Value can range from -32,768 to 32,767.
		/// </summary>
		public short LeftStickY { get; set; }

		/// <summary>
		/// The controller's right stick X-axis. Value can range from -32,768 to 32,767.
		/// </summary>
		public short RightStickX { get; set; }

		/// <summary>
		/// The controller's right stick Y-axis. Value can range from -32,768 to 32,767.
		/// </summary>
		public short RightStickY { get; set; }

		/// <summary>
		/// Generates an Xbox 360 controller report as specified here: http://free60.org/wiki/GamePad#Input_report. This can be used with ScpBus's Report() method.
		/// </summary>
		/// <returns>A 20-byte Xbox 360 controller report.</returns>
		public byte[] GetReport()
		{
			byte[] bytes = new byte[20];

			bytes[0] = 0x00;                                 // Message type (input report)
			bytes[1] = 0x14;                                 // Message size (20 bytes)

			bytes[2] = (byte)((ushort)Buttons & 0xFF);       // Buttons low
			bytes[3] = (byte)((ushort)Buttons >> 8 & 0xFF);  // Buttons high

			bytes[4] = LeftTrigger;                          // Left trigger
			bytes[5] = RightTrigger;                         // Right trigger

			bytes[6] = (byte)(LeftStickX & 0xFF);            // Left stick X-axis low
			bytes[7] = (byte)(LeftStickX >> 8 & 0xFF);       // Left stick X-axis high
			bytes[8] = (byte)(LeftStickY & 0xFF);            // Left stick Y-axis low
			bytes[9] = (byte)(LeftStickY >> 8 & 0xFF);       // Left stick Y-axis high

			bytes[10] = (byte)(RightStickX & 0xFF);          // Right stick X-axis low
			bytes[11] = (byte)(RightStickX >> 8 & 0xFF);     // Right stick X-axis high
			bytes[12] = (byte)(RightStickY & 0xFF);          // Right stick Y-axis low
			bytes[13] = (byte)(RightStickY >> 8 & 0xFF);     // Right stick Y-axis high

			// Remaining bytes are unused

			return bytes;
		}
	}

	/// <summary>
	/// The buttons to be used with an X360Controller object.
	/// </summary>
	[Flags]
	public enum X360Buttons
	{
        /// <summary>No buttons.</summary>
        None = 0,

        /// <summary>The directional pad (D-Pad) Up button.</summary>
        Up = 1 << 0,

        /// <summary>The directional pad (D-Pad) Down button.</summary>
		Down = 1 << 1,

        /// <summary>The directional pad (D-Pad) Left button.</summary>
		Left = 1 << 2,

        /// <summary>The directional pad (D-Pad) Right button.</summary>
		Right = 1 << 3,

        /// <summary>The Start button.</summary>
		Start = 1 << 4,

        /// <summary>The Back button.</summary>
		Back = 1 << 5,

        /// <summary>Clicking down on the Left Stick Button (LSB) of the left analog stick of a standard controller.</summary>
		/// <remarks>
		/// Physically, this would be triggered with the awkward movement of pushing down on the top of the left analog stick, usually via thumb.
		/// It is not recommended for games to rely on this "button" as it is not very intuitive, and generally bad for usability/accessibility.
		/// However, some games/apps do use it, so this is especially valuable to be able to simulate (especially for accessibility middleware).
		/// </remarks>
		LeftStick = 1 << 6,

        /// <summary>Clicking down on the Right Stick Button (RSB) of the right analog stick of a standard controller.</summary>
		/// <remarks>
		/// Physically, this would be triggered with the awkward movement of pushing down on the top of the right analog stick, usually via thumb.
		/// It is not recommended for games to rely on this "button" as it is not very intuitive, and generally bad for usability/accessibility.
		/// However, some games/apps do use it, so this is especially valuable to be able to simulate (especially for accessibility middleware).
		/// </remarks>
		RightStick = 1 << 7,

        /// <summary>The Left Bumper (LB).</summary>
		/// <remarks>Physically: This is the button usually located above the left analog trigger of a standard controller.</remarks>
		LeftBumper = 1 << 8,

        /// <summary>The Right Bumper (RB).</summary>
		/// <remarks>Physically: This is the button usually located above the right analog trigger of a standard controller.</remarks>
		RightBumper = 1 << 9,

        /// <summary>The Guide button.</summary>
		/// <remarks>This may sometimes be referred to as the Logo button.</remarks>
		Guide = 1 << 10,

        /// <summary>The 'A' button.</summary>
		A = 1 << 12,

        /// <summary>The 'B' button.</summary>
		B = 1 << 13,

        /// <summary>The 'X' button.</summary>
		X = 1 << 14,

        /// <summary>The 'Y' button.</summary>
		Y = 1 << 15,
    }
}

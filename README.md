# SharpXusb

A .NET Standard 2.0 library that provides direct communication with the Xbox 360 controller driver (aka XUSB driver) on Windows.

---

Some more exotic Xbox 360 controllers such as the Rock Band 3 Pro Keyboard and Pro Mustang send input data that isn't available through standard XInput. This library allows the normally-unused last 6 bytes of the XUSB input report to be read, among other things, which lets these controllers be used in their entirety.

## References and Credits

- [OpenXInput](https://github.com/Nemirtingas/OpenXinput), for a large part of how the XUSB buffers are handled
- [XInputHooker](https://github.com/nefarius/XinputHooker), for one of the control codes that OpenXinput didn't have

Other information comes from my own research and reverse-engineering attempts.

## License

This project is licensed under the MIT license. See [LICENSE](LICENSE) for details.

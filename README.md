# DrawShare ![logo](https://github.com/Rhatalin/DrawShare/blob/master/GUI/Resources/DrawShareLogo_50.png "DrawShare Logo")

DrawShare lets you connect with others so you can draw together :paintbrush:

## Idea :bulb: 

The idea came to my mind when i was talking to friends online: one of them tried to explain a situation and i thought *"wouldn't it be funny if he would quickly draw what happend?"*. I realised i didn't know an application that could do that so i started working on this little program. No matter if you plan to visually support your explanations or just want to have fun, you may want to give it a try!

## Ussage :wrench:

To connect with others one person needs to **share** his drawing board and then others can **join**.

* Share Mode: You become the host and friends can join via the shown ipaddress and port.
* Join Mode: You can assist your friend by typing in his data (ip and port).

### Local Network

Using the program in your local network is very easy as explained above.

### Global Network (also called THE INTERNET)

To use the program with others that are not in your local network an extra step is needed: Port Forwarding. Every router interface is different but this may help you:

1. Go into your router interface by typing your router-ip (gateway) into your browser.
1. Search for the feature "Port Forwarding" (likely under the tab "Security" or "Internet")
1. Fill in the form with following data:

    - Protocol: TCP
    - WAN Host IP: 0.0.0.0
    - LAN Host IP Address: \<**local** ipv4 address of your computer>
    - WAN Port: \<port>
    - LAN Host Port: \<port>
  
  If the router interface requires a range (eg. WAN Host IP Range) just type in the value twice:
  WAN Host IP Range: 0.0.0.0 ~ 0.0.0.0
  
I hope the set up wasn't to complicated and you can start drawing.
For help just ask me on twitter (https://twitter.com/Rhatalin)

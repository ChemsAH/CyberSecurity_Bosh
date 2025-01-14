from pymodbus.client import ModbusTcpClient as ModbusClient
from pymodbus.constants import Endian
from pymodbus.payload import BinaryPayloadBuilder, BinaryPayloadDecoder
import numpy as np
import time
import socket

print('Start Modbus Client')
client = ModbusClient(host='192.168.1.34', port=502)

def cutligne(block):            #Fonction pour diviser les résultats des requêtes modbus par paquet de 15 valeurs, car l'ensemble des entrées/sorties est écrit dans une liste de 15 valeurs
    n = len(block)
    lignes=[]
    for i in range(0, n, 15):
        ligne = block[i:i+15]
        lignes.append(ligne)
    return lignes


def write_ligne(l):                         #Fonction pour écrire l'état du système (I/O vector) en binaire avec en premier le temps en ms depuis le dernier changement puis toutes les sorties puis toutes les entrées
    doc = open('resultats.txt', 'at')
    str = ""
    str_udp = ""
    str += f"{l[0]}\t"
    s = (int(bin(l[0])[2:])).to_bytes(16,'little')
    for elt in l[2:10]:
        str += f"{elt:016b}"
        s = (int(bin(elt)[2:])).to_bytes(16,'little')
    str_udp = str                        # On met un \t dans le .txt mais pas dans ce qu'on envoie au code C#
    str += "\t"
    for elt in l[10:15]:
        str += f"{elt:016b}"
        str_udp += f"{elt:016b}"
        s = (int(bin(elt)[2:])).to_bytes(16,'little')
    str += "\n"
    str_udp += "\n"
    sendUDP(str_udp.encode("utf-8"))
    doc.write(str)
    doc.close()

def sendUDP(data):
    
    UDP_IP="127.0.0.1" # IP address of the Diagnosis PC
    UDP_PORT=2000 # Diagnosis Port (e.g. 2000)

    sock = socket.socket( socket.AF_INET, # Internet
                           socket.SOCK_DGRAM ) # UDP

    sock.sendto( data, (UDP_IP, UDP_PORT) )

def ecriture_120(deb_mot_prec,deb_mot):
    ajout = client.read_holding_registers(1024+deb_mot_prec,(deb_mot-deb_mot_prec)).registers        #On récupère ce qui a été écrit dans la mémoire (le 0 du pointeur correspond à 1024) avec une requête modbus
    ajout_cut = cutligne(ajout)      #On divise en listes de longueur 15 (en I/O vectors)
    #print("deb_mot_prec2 =", deb_mot_prec)
    #print("deb_mot2 =", deb_mot)
    print("reading ", (deb_mot-deb_mot_prec), "register at %MW",1024+deb_mot_prec)
    for ligne in ajout_cut:          #On écrit tous les I/O vectors dans result.txt
        write_ligne(ligne)
        #print(ligne)
        
    

doc = open('resultats.txt', 'wt')
doc.close
stack = 15000
client.write_register(600,stack)        #On fixe la stack pour la mémoire tournante, entre 1024 et 1024 + 15000
client.write_register(602, 0)           #On fixe le début de l'écriture au début de la mémoire tournante (pointeur à 0)
client.write_register(603, 1)
deb_mot = client.read_holding_registers(602,1).registers[0]
deb_mot_prec = deb_mot                                          #On initialise la variable deb_mot_prec qui sert à savoir à quel endroit de la mémoire tournante on était à la dernière observation pour savoir quelles sont les valeurs à demander en modbus
ecriture_120(deb_mot_prec-15,deb_mot_prec)
#client.write_register(deb_mot_prec+1024, 5)
#print(client.read_holding_registers(deb_mot_prec,30).registers[:])

test = client.read_holding_registers(603,1).registers[0]
ajout = []                                                      #Initialisation du I/O vector que l'on va écrire dans result.txt
while 1:
    if deb_mot > deb_mot_prec:           #On regarde s'il y a eu un ajout dans la mémoire tournante (on regarde si le pointeur a changé)
        #print("deb_mot =", deb_mot)
        #print("deb_mot_prec =", deb_mot_prec)
        diff = (deb_mot - deb_mot_prec)
        nbr_multiple = diff//120
        reste_multiple = diff%120
        if diff>120:
            for i in range(0,diff-120,120):               #Sécurité pour éviter une requête de + de 125 valeurs en modbus, on divise par tranche de 125 qu'on écrit les unes après les autres
                #print("coucou",i)
                deb_mot_prec_incr = deb_mot_prec + i    
                deb_mot_incr = deb_mot_prec_incr + 120
                ecriture_120(deb_mot_prec_incr,deb_mot_incr)
        deb_mot_prec_incr = deb_mot_prec + (nbr_multiple)*120  #Cas de la dernière tranche qui est de longueur reste_multiple
        deb_mot_incr = deb_mot
        ecriture_120(deb_mot_prec_incr,deb_mot_incr)
        print(deb_mot_prec+1024, "-", f"{-deb_mot_prec+deb_mot:03d}")
            
    elif deb_mot < deb_mot_prec:         #Cas particulier si on est arrivé au bout de la mémoire tournante
        #print("deb_mot_elif =", deb_mot)
        #print("deb_mot_prec_elif =", deb_mot_prec)
        diff = stack - (deb_mot_prec - deb_mot)
        nbr_multiple = diff//120
        reste_multiple = diff%120
        for i in range(0,diff-120,120):               #Sécurité pour éviter une requête de + de 125 valeurs en modbus, on divise par tranche de 125 qu'on écrit les unes après les autres
            #print("coucou",i)
            #print("deb_mot_prec =", deb_mot_prec)
            #print("deb_mot =", deb_mot)
            deb_mot_prec_incr = deb_mot_prec + i
            deb_mot_incr = deb_mot_prec_incr + 120
            if deb_mot_incr < stack:
                ecriture_120(deb_mot_prec_incr,deb_mot_incr)
            else:
                #print("retour mémoire, deb_mot_incr-15000 =",deb_mot_incr-15000)
                #print("deb_mot_incr =", deb_mot_incr)
                ecriture_120(deb_mot_prec_incr,15000)
                ecriture_120(0,deb_mot_incr-15000)
        deb_mot_prec_incr = deb_mot_prec + nbr_multiple*120 
        deb_mot_incr = deb_mot
        #print("deb_mot_prec_incr =", deb_mot_prec_incr)
        #print("deb_mot_incr =", deb_mot_incr)
        if (deb_mot_incr-deb_mot_prec_incr) > 0:
            ecriture_120(deb_mot_prec_incr,deb_mot_incr)
        else:
            ecriture_120(deb_mot_prec_incr,15000)
            ecriture_120(0,deb_mot_incr)
        
    deb_mot_prec = deb_mot               #On met à jour le pointeur de la mémoire lue
    time.sleep(0.1)                      #On attend 0.1sec, choix de temps arbitraire (même que celui du code original), cela permet de ne pas avoir trop de changements par itération de la boucle sans avoir trop d'itérations non plus
    deb_mot = client.read_holding_registers(602,1).registers[0]         #On lit le nouveau pointeur de la mémoire



# Fermeture du client
client.close()


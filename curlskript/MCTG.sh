#!/bin/sh

# --------------------------------------------------
# Monster Trading Cards Game
# --------------------------------------------------
echo "CURL Testing for Monster Trading Cards Game"
echo "Syntax: MonsterTradingCards.sh [pause]"
echo "- pause: optional, if set, then script will pause after each block"
echo .

pauseFlag=0
for arg in "$@"; do
    if [ "$arg" == "pause" ]; then
        pauseFlag=1 
        break
    fi
done

if [ $pauseFlag -eq 1 ]; then read -p "Press enter to continue..."; fi

# --------------------------------------------------
echo "1) Create Users (Registration)"
# Create User
curl -i -X POST http://localhost:10001/api/auth/register --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\", \"Role\":\"Player\"}"
echo "Should return HTTP 201"
echo .
curl -i -X POST http://localhost:10001/api/auth/register --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\", \"Role\":\"Player\"}"
echo "Should return HTTP 201"
echo .
curl -i -X POST http://localhost:10001/api/auth/register --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\", \"Role\":\"Admin\"}"
echo "Should return HTTP 201"
echo .

if [ $pauseFlag -eq 1 ]; then read -p "Press enter to continue..."; fi

echo "should fail:"
curl -i -X POST http://localhost:10001/api/auth/register --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\", \"Role\":\"Player\"}"
echo "Should return HTTP 4xx - User already exists"
echo .
curl -i -X POST http://localhost:10001/api/auth/register --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\", \"Role\":\"Player\"}"
echo "Should return HTTP 4xx - User already exists"
echo . 
echo .

if [ $pauseFlag -eq 1 ]; then read -p "Press enter to continue..."; fi

echo "2) Login Users"
# Login User and store token
token_kienboec=$(curl -s -X POST http://localhost:10001/api/auth/login --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}" | grep -o '"token":"[^"]*' | grep -o '[^"]*$')
echo "Token for kienboec: $token_kienboec"
echo .
token_altenhof=$(curl -s -X POST http://localhost:10001/api/auth/login --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}" | grep -o '"token":"[^"]*' | grep -o '[^"]*$')
echo "Token for altenhof: $token_altenhof"
echo .
token_admin=$(curl -s -X POST http://localhost:10001/api/auth/login --header "Content-Type: application/json" -d "{\"Username\":\"admin\", \"Password\":\"istrator\"}" | grep -o '"token":"[^"]*' | grep -o '[^"]*$')
echo "Token for admin: $token_admin"
echo .


if [ $pauseFlag -eq 1 ]; then read -p "Press enter to continue..."; fi

echo "should fail:"
curl -i -X POST http://localhost:10001/api/auth/login --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo "Should return HTTP 4xx - Login failed"
echo .
echo .

if [ $pauseFlag -eq 1 ]; then read -p "Press enter to continue..."; fi

# Get all users
echo "3) Get all users (done by admin)"
curl -i -X GET http://localhost:10001/api/users --header "Authorization: $token_admin"
echo "Should return HTTP 200 with list of users"
echo .

echo "should fail:"
curl -i -X GET http://localhost:10001/api/users --header "Authorization: $token_kienboec"
echo "Should return HTTP 401 - Unauthorized"
echo .


if [ $pauseFlag -eq 1 ]; then read -p "Press enter to continue..."; fi

# --------------------------------------------------
echo "4) create packages (done by admin)"

curl -i -X POST http://localhost:10001/api/packages --header "Content-Type: application/json" --header "Authorization: $token_admin" -d "[{\"Name\":\"Goblin\", \"Damage\": 10.0, \"ElementType\":\"Water\",\"CardType\":\"Monster\"}, {\"Name\":\"Dragon\", \"Damage\": 50.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 20.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}, {\"Name\":\"Ork\", \"Damage\": 45.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 25.0, \"ElementType\":\"Fire\",\"CardType\":\"Spell\"}]"
echo "Should return HTTP 201"
echo .

curl -i -X POST http://localhost:10001/api/packages --header "Content-Type: application/json" --header "Authorization: $token_admin" -d "[{\"Name\":\"Goblin\", \"Damage\": 9.0, \"ElementType\":\"Water\",\"CardType\":\"Monster\"}, {\"Name\":\"Dragon\", \"Damage\": 55.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 21.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}, {\"Name\":\"Ork\", \"Damage\": 55.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 23.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}]"
echo "Should return HTTP 201"
echo .

curl -i -X POST http://localhost:10001/api/packages --header "Content-Type: application/json" --header "Authorization: $token_admin" -d "[{\"Name\":\"Goblin\", \"Damage\": 11.0, \"ElementType\":\"Water\",\"CardType\":\"Monster\"}, {\"Name\":\"Dragon\", \"Damage\": 70.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 22.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}, {\"Name\":\"Ork\", \"Damage\": 40.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 28.0, \"ElementType\":\"Normal\",\"CardType\":\"Spell\"}]"
echo "Should return HTTP 201"
echo .

curl -i -X POST http://localhost:10001/api/packages --header "Content-Type: application/json" --header "Authorization: $token_admin" -d "[{\"Name\":\"Goblin\", \"Damage\": 10.0, \"ElementType\":\"Water\",\"CardType\":\"Monster\"}, {\"Name\":\"Dragon\", \"Damage\": 50.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 20.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}, {\"Name\":\"Ork\", \"Damage\": 45.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 25.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}]"
echo "Should return HTTP 201"
echo .

curl -i -X POST http://localhost:10001/api/packages --header "Content-Type: application/json" --header "Authorization: $token_admin" -d "[{\"Name\":\"Goblin\", \"Damage\": 9.0, \"ElementType\":\"Water\",\"CardType\":\"Monster\"}, {\"Name\":\"Dragon\", \"Damage\": 55.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 21.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}, {\"Name\":\"Ork\", \"Damage\": 55.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 23.0, \"ElementType\":\"Fire\",\"CardType\":\"Spell\"}]"
echo "Should return HTTP 201"
echo .

curl -i -X POST http://localhost:10001/api/packages --header "Content-Type: application/json" --header "Authorization: $token_admin" -d "[{\"Name\":\"Goblin\", \"Damage\": 11.0, \"ElementType\":\"Water\",\"CardType\":\"Monster\"}, {\"Name\":\"Dragon\", \"Damage\": 70.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 22.0, \"ElementType\":\"Water\",\"CardType\":\"Spell\"}, {\"Name\":\"Ork\", \"Damage\": 40.0, \"ElementType\":\"Normal\",\"CardType\":\"Monster\"}, {\"Name\":\"Spell\", \"Damage\": 28.0, \"ElementType\":\"Normal\",\"CardType\":\"Spell\"}]"
echo "Should return HTTP 201"
echo .
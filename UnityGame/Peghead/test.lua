---- Prints out num sold required for every badge
--local Items = require(game.ReplicatedStorage.Common.Game.Deps.Items)
--local BadgeUtil = require(game.ReplicatedStorage.Common.Game.Util.BadgeUtil) 

--for _, item in Items.getOfType("JunkItem") do
--	print(item.Name)
--	print(BadgeUtil.getBadgeNumSold(item.Name, BadgeUtil.Tiers.Bronze))
--	print(BadgeUtil.getBadgeNumSold(item.Name, BadgeUtil.Tiers.Silver))
--	print(BadgeUtil.getBadgeNumSold(item.Name, BadgeUtil.Tiers.Gold))
--	print()
--end

local Classes = require(game.ReplicatedStorage.Classes.Package.ClassesServer)
local ServerStorage = game:GetService("ServerStorage")
local ReplicatedStorage = game:GetService("ReplicatedStorage")
local Plums = require(game.ReplicatedStorage.Common.Systems.Plums.Package.PlumsServer)
local Knit = require(game.ReplicatedStorage.Common.Packages.Knit)
local Animations = require(game.ReplicatedStorage.Common.Systems.Animations.Package.AnimationsServer)
local Episodes = require(game.ReplicatedStorage.Common.Systems.Episodes.Package.EpisodesServer)
local Viz = require(game.ReplicatedStorage.Common.Systems.Viz.Package.VizServer)

local function requireChildren(parent)
	for _, v in ipairs(parent:GetChildren()) do
		if v:IsA("ModuleScript") then
			require(v)
		end
	end
end

Viz:Init()
Plums:Init()
Animations:Init({
	AutoRegisterPlayers = true,
	AutoLoadAllPlayerTracks = true,
	AnimatedObjectsDebugMode = false,
	TimeToLoadPrints = false
})
Episodes:Init()

requireChildren(script.LoadBeforeKnit)

Classes.addChildren(script.Classes)

Knit.AddServices(script.Services)
Knit:Start():andThen(function()
	--requireChildren(script.Components)
	--requireChildren(script.Components.Vehicles)
	print("Server: Knit started and components required")
end)
SELECT 
    Name, 
    COUNT(*) AS Occurrence,
    ROUND(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Skins), 2) AS Percentage
FROM Skins
GROUP BY Name
ORDER BY Occurrence DESC
LIMIT 5;
INSERT INTO `edbgs`.`app_sfhist`
(`id`,
`Datecycle`,
`fInfluence`,
`sFaction`,
`sPopulation`,
`fhState_id`,
`sGovtype_id`,
`sSecurity_id`,
`seconomy_id`,
`sfhFaction_id`,
`sfhSystem_id`,
`shState_id`,
`spower_id`,
`spower_state_id`)
VALUES
(<{id}>,
<{Datecycle}>,
<{fInfluence}>,
<{sFaction}>,
<{sPopulation}>,
<{fhState_id}>,
<{sGovtype_id}>,
<{sSecurity_id}>,
<{seconomy_id}>,
<{sfhFaction_id}>,
<{sfhSystem_id}>,
<{shState_id}>,
<{spower_id}>,
<{spower_state_id}>);


UPDATE `edbgs`.`app_sfhist`
SET
`id` = <{id}>,
`Datecycle` = <{Datecycle}>,
`fInfluence` = <{fInfluence}>,
`sFaction` = <{sFaction}>,
`sPopulation` = <{sPopulation}>,
`fhState_id` = <{fhState_id}>,
`sGovtype_id` = <{sGovtype_id}>,
`sSecurity_id` = <{sSecurity_id}>,
`seconomy_id` = <{seconomy_id}>,
`sfhFaction_id` = <{sfhFaction_id}>,
`sfhSystem_id` = <{sfhSystem_id}>,
`shState_id` = <{shState_id}>,
`spower_id` = <{spower_id}>,
`spower_state_id` = <{spower_state_id}>
WHERE <{where_expression}>;



SELECT `id`, `Datecycle`, `fInfluence`, `sFaction`, `sPopulation`, `fhState_id`, `sGovtype_id`, `sSecurity_id`, `seconomy_id`, `sfhFaction_id`, `sfhSystem_id`, `shState_id`, `spower_id`, `spower_state_id` FROM `edbgs`.`app_sfhist`;

`id`, `Datecycle`, `fInfluence`, `sFaction`, `sPopulation`, `fhState_id`, `sGovtype_id`, `sSecurity_id`, `seconomy_id`, `sfhFaction_id`, `sfhSystem_id`, `shState_id`, `spower_id`, `spower_state_id`

`app_sfhist`.`id`, `app_sfhist`.`Datecycle`, `app_sfhist`.`fInfluence`, `app_sfhist`.`sFaction`, `app_sfhist`.`sPopulation`, `app_sfhist`.`fhState_id`, `app_sfhist`.`sGovtype_id`, `app_sfhist`.`sSecurity_id`, `app_sfhist`.`seconomy_id`, `app_sfhist`.`sfhFaction_id`, `app_sfhist`.`sfhSystem_id`, `app_sfhist`.`shState_id`, `app_sfhist`.`spower_id`, `app_sfhist`.`spower_state_id`
package uk.co.emote;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Table(name = "competencycompetencylink")
public class CompetencyCompetencyLink {

	@Id
	// @Id indicates that this it a unique primary key
	@GeneratedValue
	// @GeneratedValue indicates that value is automatically generated by the
	// server
	private int id;

	@Column
	private String parentCompetencyName;

	@Column
	private String childCompetencyName;

	@Column
	private int weight;

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getParentCompetencyName() {
		return parentCompetencyName;
	}

	public void setParentCompetencyName(String parentCompetencyName) {
		this.parentCompetencyName = parentCompetencyName;
	}

	public String getChildCompetencyName() {
		return childCompetencyName;
	}

	public void setChildCompetencyName(String childCompetencyName) {
		this.childCompetencyName = childCompetencyName;
	}

	public int getWeight() {
		return weight;
	}

	public void setWeight(int weight) {
		this.weight = weight;
	}

}

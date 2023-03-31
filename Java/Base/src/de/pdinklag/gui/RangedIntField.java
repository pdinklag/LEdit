package de.pdinklag.gui;

/**
 * A text field accepting input of integer that are in a certain range.
 */
public class RangedIntField extends IntField {
    private static final long serialVersionUID = 7877004802095940987L;

    private int min, max;

    public RangedIntField(int min, int max) {
        this.min = min;
        this.max = max;
    }

    @Override
    protected Integer parse(String text) {
        Integer i = super.parse(text);
        if (i != null) {
            if (i >= min && i <= max)
                return i;
            else
                return null;
        } else {
            return null;
        }
    }

    public int getMin() {
        return min;
    }

    public void setMin(int min) {
        this.min = min;
    }

    public int getMax() {
        return max;
    }

    public void setMax(int max) {
        this.max = max;
    }
}
